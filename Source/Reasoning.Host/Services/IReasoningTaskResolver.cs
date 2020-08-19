using System;
using System.Threading;
using System.Threading.Tasks;
using Reasoning.Core.Contracts;

namespace Reasoning.Host.Services
{
    /// <summary>
    /// Contract for Reasoning Task Resolving Service
    /// </summary>
    public interface IReasoningTaskResolver
    {
        /// <summary>
        /// Builds async task function
        /// </summary>
        /// <param name="reasoningTaskId">Reasoning Task Id</param>
        /// <returns>Async task function</returns>
        Func<CancellationToken, Task> BuildBackgroundTask(string reasoningTaskId);
        /// <summary>
        /// Tries to enqueue existing Reasoning Task by Id
        /// </summary>
        /// <param name="reasoningTaskId">Reasoning Task Id</param>
        void EnqueueReasoningTask(string reasoningTaskId);
        /// <summary>
        /// Tries to process reasoning
        /// </summary>
        /// <param name="reasoningTaskId">Reasoning Task Id</param>
        /// <returns>Processed Reasoning Task</returns>
        Task<IReasoningTask> ProcessReasoningTask(string reasoningTaskId);
    }
}
