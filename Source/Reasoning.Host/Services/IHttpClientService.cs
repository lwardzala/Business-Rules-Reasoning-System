using System.Threading.Tasks;

using Reasoning.Core.Contracts;

namespace Reasoning.Host.Services
{
    /// <summary>
    /// Contract for HTTP Client Service
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Executes reasoning HTTP request with response content
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="reasoningRequest">HTTP request object</param>
        /// <returns>Response content T</returns>
        Task<T> ExecuteTaskAsync<T>(IReasoningRequest reasoningRequest);
        /// <summary>
        /// Executes reasoning HTTP request without response content
        /// </summary>
        /// <param name="reasoningRequest">HTTP request object</param>
        /// <returns>void</returns>
        Task ExecuteTaskAsync(IReasoningRequest reasoningRequest);
    }
}
