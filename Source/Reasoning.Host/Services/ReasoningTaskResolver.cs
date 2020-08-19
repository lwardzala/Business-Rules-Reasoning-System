using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Reasoning.Core.Contracts;
using Reasoning.MongoDb.Repositories;
using Reasoning.Host.Repositories;
using Reasoning.Host.Resources;

namespace Reasoning.Host.Services
{
    public class ReasoningTaskResolver : IReasoningTaskResolver
    {
        private readonly IReasoningTaskRepository _reasoningTaskRepository;
        private readonly IReasoningService _reasoningService;
        private readonly IBackgroundTaskQueue _taskQueueRepository;
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<ReasoningTaskResolver> _logger;

        public ReasoningTaskResolver(
            IReasoningTaskRepository reasoningTaskRepository,
            IReasoningService reasoningService,
            IBackgroundTaskQueue taskQueue,
            IHttpClientService httpClientService,
            ILogger<ReasoningTaskResolver> logger
            )
        {
            _reasoningTaskRepository = reasoningTaskRepository;
            _reasoningService = reasoningService;
            _taskQueueRepository = taskQueue;
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public Func<CancellationToken, Task> BuildBackgroundTask(string reasoningTaskId)
        {
            return async token =>
            {
                _logger.LogInformation($"Reasoning task {reasoningTaskId} has been dequeued. Reasoning process is starting");

                var reasoningTask = await ProcessReasoningTask(reasoningTaskId);

                _logger.LogInformation($"Reasoning task {reasoningTaskId} execution has been finished with result {reasoningTask.Status}");
            };
        }

        public void EnqueueReasoningTask(string reasoningTaskId)
        {
            _taskQueueRepository.QueueBackgroundWorkItem(BuildBackgroundTask(reasoningTaskId));

            _logger.LogInformation($"Reasoning task {reasoningTaskId} has been enqueued");
        }

        public async Task<IReasoningTask> ProcessReasoningTask(string reasoningTaskId)
        {
            var reasoningTask = await _reasoningTaskRepository.GetAsync(reasoningTaskId);

            reasoningTask = await ContinueReasoningStepByStep(reasoningTask);

            SetTaskState(ref reasoningTask);

            _reasoningTaskRepository.Update(reasoningTaskId, reasoningTask);

            if (reasoningTask.ReasoningProcess.State == ReasoningState.FINISHED
                && reasoningTask.ReasoningProcess.EvaluationMessage == EvaluationMessage.PASSED
                && reasoningTask.ReasoningProcess.ReasonedItems.Count > 0)
            {
                ExecuteReasoningTasks(reasoningTask);
            }

            return reasoningTask;
        }

        private async Task<IReasoningTask> ContinueReasoningStepByStep(IReasoningTask reasoningTask)
        {
            reasoningTask.ReasoningProcess = _reasoningService.ContinueReasoning(reasoningTask.ReasoningProcess);

            IList<IVariableSource> sources = reasoningTask.Sources?.ToList() ?? new List<IVariableSource>();
            IList<string> missingVariables = _reasoningService.GetAllMissingVariableIds(reasoningTask.ReasoningProcess).ToList();

            while (CanContinueReasoning(reasoningTask.ReasoningProcess, sources, missingVariables))
            {
                var missingVariableId = missingVariables.First();

                var source = sources.FirstOrDefault(src => src.VariableIds.Contains(missingVariableId));
                var variablesResource = await _httpClientService.ExecuteTaskAsync<VariablesResource>(source?.Request);
                sources.Remove(source);

                if (variablesResource?.Variables == null)
                {
                    _logger.LogError($"Couldn't parse values from {source.Request.Uri}");
                    continue;
                }
                
                variablesResource.Variables.ToList().ForEach(variable => missingVariables.Remove(variable.Id));

                reasoningTask.ReasoningProcess = _reasoningService.SetValues(reasoningTask.ReasoningProcess, variablesResource.Variables);
                reasoningTask.ReasoningProcess = _reasoningService.ContinueReasoning(reasoningTask.ReasoningProcess);
            }

            return reasoningTask;
        }

        private bool CanContinueReasoning(IReasoningProcess reasoningProcess, IList<IVariableSource> sources, IList<string> missingVariables)
        {
            if (reasoningProcess.State == ReasoningState.FINISHED) return false;
            if (sources.Count == 0) return false;
            var missingVariableId = missingVariables.First();
            if (sources.Any(src => !src.VariableIds.Contains(missingVariableId))) return false;

            return true;
        }

        private void ExecuteReasoningTasks(IReasoningTask reasoningTask)
        {
            _logger.LogInformation($"Analyzing reasoning results");

            reasoningTask.Actions?.ToList().ForEach(action =>
            {
                if (!reasoningTask.ReasoningProcess.ReasonedItems.Any(item => action.ReasoningItems.Contains(item))) return;

                _logger.LogInformation($"Reasoning actions execution has started");

                action.ReasoningRequests?.ToList().ForEach(request =>
                {
                    _httpClientService.ExecuteTaskAsync(request);
                });
            });
        }

        private void SetTaskState(ref IReasoningTask reasoningTask)
        {
            var reasoningProcess = reasoningTask.ReasoningProcess;

            _logger.LogInformation($"Reasoning has been finished with result {reasoningProcess.State} {reasoningProcess.EvaluationMessage}");

            switch (reasoningProcess.State)
            {
                case ReasoningState.FINISHED:
                    reasoningTask.Status = ReasoningTaskStatus.FINISHED;
                    break;
                case ReasoningState.STOPPED:
                    reasoningTask.Status = ReasoningTaskStatus.STOPPED;
                    break;
            }
        }
    }
}
