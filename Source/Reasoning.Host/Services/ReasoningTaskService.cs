using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Reasoning.Core;
using Reasoning.Core.Contracts;
using Reasoning.Host.Resources;
using Reasoning.MongoDb;
using Reasoning.MongoDb.Repositories;

namespace Reasoning.Host.Services
{
    public class ReasoningTaskService : IReasoningTaskService
    {
        private readonly IReasoningTaskRepository _reasoningTaskRepository;
        private readonly IKnowledgeBaseRepository _knowledgeBaseRepository;
        private readonly IReasoningService _reasoningService;
        private readonly IReasoningTaskResolver _reasoningTaskResolver;
        private readonly ILogger<ReasoningTaskService> _logger;

        public ReasoningTaskService(
            IReasoningTaskRepository reasoningTaskRepository,
            IKnowledgeBaseRepository knowledgeBaseRepository,
            IReasoningService reasoningService,
            IReasoningTaskResolver reasoningTaskResolver,
            ILogger<ReasoningTaskService> logger
            )
        {
            _reasoningTaskRepository = reasoningTaskRepository;
            _knowledgeBaseRepository = knowledgeBaseRepository;
            _reasoningService = reasoningService;
            _reasoningTaskResolver = reasoningTaskResolver;
            _logger = logger;
        }

        public async Task<ReasoningTaskResource> CancelTaskAsync(string id)
        {
            var result = await _reasoningTaskRepository.GetAsync(id);
            

            if (result == null) return null;

            result.Status = ReasoningTaskStatus.CANCELLED;

            _reasoningTaskRepository.Update(id, result);

            _logger.LogInformation($"Reasoning task {id} has been cancelled");

            return GetReasoningTaskResource(result);
        }

        public async Task<ReasoningTaskResource> CreateTaskAsync(CreateReasoningTaskResource reasoningTaskResource)
        {
            var knowledgeBase = await _knowledgeBaseRepository.GetAsync(reasoningTaskResource.KnowledgeBaseId);

            if (knowledgeBase == null)
            {
                var message = $"Couldn't find knowledge base with id `{reasoningTaskResource.KnowledgeBaseId}`";

                _logger.LogInformation(message);
                throw new Exception(message);
            }

            var reasoningProcess = ReasoningProcessFactory.CreateInstance(
                await _knowledgeBaseRepository.GetAsync(reasoningTaskResource.KnowledgeBaseId),
                reasoningTaskResource.ReasoningMethod,
                reasoningTaskResource.Hypothesis
                );

            var result = ReasoningTaskFactory.CreateInstance(
                _reasoningService.ClearReasoning(reasoningProcess),
                reasoningTaskResource.Description,
                reasoningTaskResource.Sources,
                reasoningTaskResource.Actions
                );

            _reasoningTaskRepository.Create(result);

            _logger.LogInformation($"Reasoning task {result.Id} has been created");

            _reasoningTaskResolver.EnqueueReasoningTask(result.Id);

            return GetReasoningTaskResource(result);
        }

        public async Task<bool> DeleteTaskAsync(string id)
        {
            var result = await _reasoningTaskRepository.RemoveAsync(id);

            _logger.LogInformation($"Reasoning task {id} has been deleted");

            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<IReasoningTask>> GetAllWaitingTasksAsync()
        {
            return (await _reasoningTaskRepository.GetAsync(task => task.Status == ReasoningTaskStatus.WAITING, 3000)).AsEnumerable();
        }

        public async Task<ReasoningTaskResource> GetTaskAsync(string id)
        {
            return GetReasoningTaskResource(await _reasoningTaskRepository.GetAsync(id));
        }

        public async Task<DetailedReasoningTaskResource> GetTaskDetailAsync(string id)
        {
            return new DetailedReasoningTaskResource { ReasoningTask = await _reasoningTaskRepository.GetAsync(id) };
        }

        public async Task<ReasoningTaskResource> ResumeTaskAsync(string id)
        {
            var result = await _reasoningTaskRepository.GetAsync(id);

            if (result == null) return null;

            result.Status = ReasoningTaskStatus.WAITING;

            _reasoningTaskRepository.Update(id, result);

            _logger.LogInformation($"Reasoning task {id} has been resumed");

            _reasoningTaskResolver.EnqueueReasoningTask(id);

            return GetReasoningTaskResource(result);
        }

        public async Task<MissingVariablesResource> SetVariablesAsync(string id, VariablesResource variablesResource)
        {
            var reasoningTask = await _reasoningTaskRepository.GetAsync(id);

            if (reasoningTask == null) return null;

            reasoningTask.ReasoningProcess = _reasoningService.SetValues(reasoningTask.ReasoningProcess, variablesResource.Variables);            
            reasoningTask.Status = ReasoningTaskStatus.WAITING;

            _reasoningTaskRepository.Update(id, reasoningTask);

            _logger.LogInformation($"variables {variablesResource.Variables} have been set to reasoning task {id}");

            _reasoningTaskResolver.EnqueueReasoningTask(id);

            var result = _reasoningService.GetAllMissingVariableIds(reasoningTask.ReasoningProcess);

            return new MissingVariablesResource { MissingVariableIds = result };
        }

        private ReasoningTaskResource GetReasoningTaskResource(IReasoningTask reasoningTask)
        {
            return new ReasoningTaskResource
            {
                Id = reasoningTask.Id,
                Description = reasoningTask.Description,
                KnowledgeBaseId = reasoningTask.KnowledgeBaseId,
                ReasoningMethod = reasoningTask.ReasoningMethod,
                Status = reasoningTask.Status,
                EvaluationMessage = reasoningTask.ReasoningProcess.EvaluationMessage,
                MissingVariableIds = reasoningTask.Status == ReasoningTaskStatus.STOPPED 
                    ? _reasoningService.GetAllMissingVariableIds(reasoningTask.ReasoningProcess)
                    : null,
                ReasonedItems = reasoningTask.ReasoningProcess.ReasonedItems
            };
        }
    }
}
