using System.Linq;
using Microsoft.Extensions.Logging;

namespace Reasoning.Host.Services
{
    /// <summary>
    /// Reasoning task initiator for retrieving unresolved tasks from database
    /// </summary>
    public class Initiator
    {
        private readonly IReasoningTaskService _reasoningTaskService;
        private readonly IReasoningTaskResolver _reasoningTaskResolver;
        private readonly ILogger<Initiator> _logger;

        public Initiator(IReasoningTaskService reasoningTaskService,
            IReasoningTaskResolver reasoningTaskResolver,
            ILogger<Initiator> logger
            )
        {
            _reasoningTaskService = reasoningTaskService;
            _reasoningTaskResolver = reasoningTaskResolver;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves unresolved tasks from database
        /// </summary>
        public void RetrieveTasks()
        {
            _logger.LogInformation($"Retrieving waiting tasks");

            var tasks = _reasoningTaskService.GetAllWaitingTasksAsync().Result;

            tasks.ToList().ForEach(task =>
            {
                _reasoningTaskResolver.EnqueueReasoningTask(task.Id);
            });

            _logger.LogInformation($"Waiting tasks have been retrieved");
        }
    }
}
