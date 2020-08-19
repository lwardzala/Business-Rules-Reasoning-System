using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reasoning.Host.Repositories
{
    /// <summary>
    /// Interface of Task Queue
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Enqueues working item asynchronously
        /// </summary>
        /// <param name="workItem">Async task function</param>
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
        /// <summary>
        /// Dequeues working item asynchronously
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Async task function</returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
