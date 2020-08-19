using System.Threading.Tasks;

using Reasoning.Core.Contracts;
using Reasoning.Host.Resources;

namespace Reasoning.Host.Services
{
    /// <summary>
    /// Contract for Knowledge Base Service
    /// </summary>
    public interface IKnowledgeBaseService
    {
        /// <summary>
        /// Gets Knowledge Base by Id
        /// </summary>
        /// <param name="id">Knowledge Base Id</param>
        /// <returns>Knowledge Base Resource</returns>
        Task<KnowledgeBaseResource> GetAsync(string id);
        /// <summary>
        /// Creates Knowledge Base
        /// </summary>
        /// <param name="knowledgeBase">Knowledge Base object</param>
        /// <returns>Knowledge Base Resource</returns>
        Task<KnowledgeBaseResource> CreateAsync(IKnowledgeBase knowledgeBase);
        /// <summary>
        /// Deletes Knowledge Base by Id
        /// </summary>
        /// <param name="id">Knowledge Base Id</param>
        /// <returns>True or False</returns>
        Task<bool> DeleteAsync(string id);
        /// <summary>
        /// Updates existing Knowledge Base
        /// </summary>
        /// <param name="id">Knowledge Base Id</param>
        /// <param name="knowledgeBase">Knowledge Base object</param>
        /// <returns>Knowledge Base Resource</returns>
        Task<KnowledgeBaseResource> UpdateAsync(string id, IKnowledgeBase knowledgeBase);
    }
}
