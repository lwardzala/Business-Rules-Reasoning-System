using System.Collections.Generic;
using System.Threading.Tasks;

using Reasoning.Core.Contracts;
using Reasoning.Host.Resources;

namespace Reasoning.Host.Services
{
    /// <summary>
    /// Contract for Reasoning Task Service
    /// </summary>
    public interface IReasoningTaskService
    {
        /// <summary>
        /// Creates and enqueues Reasoning Task asynchronously
        /// </summary>
        /// <param name="reasoningTaskResource">Reasoning Task Resource</param>
        /// <returns>Reasoning Task Resource</returns>
        Task<ReasoningTaskResource> CreateTaskAsync(CreateReasoningTaskResource reasoningTaskResource);
        /// <summary>
        /// Gets Reasoning Task by Id asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <returns>Reasoning Task Resource</returns>
        Task<ReasoningTaskResource> GetTaskAsync(string id);
        /// <summary>
        /// Gets detailed Reasoning Task by Id asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <returns>Reasoning Task Resource</returns>
        Task<DetailedReasoningTaskResource> GetTaskDetailAsync(string id);
        /// <summary>
        /// Tries to set variables to Reasoning Process and enqueue Reasoning Task asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <param name="variablesResource">Variables Resource</param>
        /// <returns>Missing Variables Resource</returns>
        Task<MissingVariablesResource> SetVariablesAsync(string id, VariablesResource variablesResource);
        /// <summary>
        /// Deletes Reasoning Task by Id asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <returns>True or False</returns>
        Task<bool> DeleteTaskAsync(string id);
        /// <summary>
        /// Tries to resume and enqueue Reasoning Task asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <returns>Reasoning Task Resource</returns>
        Task<ReasoningTaskResource> ResumeTaskAsync(string id);
        /// <summary>
        /// Cancels Reasoning Task asynchronously
        /// </summary>
        /// <param name="id">Reasoning Task Id</param>
        /// <returns>Reasoning Task Resource</returns>
        Task<ReasoningTaskResource> CancelTaskAsync(string id);
        /// <summary>
        /// Gets all Reasoning Tasks with status WAITING asynchronously
        /// </summary>
        /// <returns>Collection of Reasoning Tasks</returns>
        Task<IEnumerable<IReasoningTask>> GetAllWaitingTasksAsync();
    }
}
