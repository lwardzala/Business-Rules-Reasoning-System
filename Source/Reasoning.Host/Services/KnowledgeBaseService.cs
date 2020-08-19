using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Reasoning.Core.Contracts;
using Reasoning.Host.Resources;
using Reasoning.MongoDb.Repositories;

namespace Reasoning.Host.Services
{
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly IKnowledgeBaseRepository _knowledgeBaseRepository;
        private readonly ILogger<KnowledgeBaseService> _logger;

        public KnowledgeBaseService(IKnowledgeBaseRepository knowledgeBaseRepository, ILogger<KnowledgeBaseService> logger)
        {
            _knowledgeBaseRepository = knowledgeBaseRepository;
            _logger = logger;
        }

        public async Task<KnowledgeBaseResource> CreateAsync(IKnowledgeBase knowledgeBase)
        {
            _logger.LogInformation($"Creating knowledge base {knowledgeBase.Id}");

            if (await CountDocumentsByIdAsync(knowledgeBase.Id) > 0)
            {
                _logger.LogWarning($"Knowledge base {knowledgeBase.Id} already exists");

                return null;
            }

            _knowledgeBaseRepository.Create(knowledgeBase);

            _logger.LogInformation($"Knowledge base {knowledgeBase.Id} has been created");

            return new KnowledgeBaseResource { KnowledgeBase = knowledgeBase };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation($"Trying to delete knowledge base {id}");

            var result = await _knowledgeBaseRepository.RemoveAsync(id);

            if (result.DeletedCount > 0) _logger.LogInformation($"Knowledge base {id} has been deleted");
            else _logger.LogWarning($"Couldn't delete knowledge base {id}");

            return result.DeletedCount > 0;
        }

        public async Task<KnowledgeBaseResource> GetAsync(string id)
        {
            _logger.LogInformation($"Trying to get knowledge base {id}");

            var knowledgeBase = await _knowledgeBaseRepository.GetAsync(id);

            if (knowledgeBase == null)
            {
                _logger.LogWarning($"Couldn't find knowledge base {id}");

                return null;
            }

            _logger.LogInformation($"Knowledge base {id} has been found");

            return new KnowledgeBaseResource { KnowledgeBase = knowledgeBase };
        }

        public async Task<KnowledgeBaseResource> UpdateAsync(string id, IKnowledgeBase knowledgeBase)
        {
            _logger.LogInformation($"Trying to update knowledge base {id}");

            if (id != knowledgeBase.Id)
            {
                var message = "Can't change or null document id";

                _logger.LogWarning(message);

                throw new Exception(message);
            }

            var result = await _knowledgeBaseRepository.UpdateAsync(id, knowledgeBase);

            if (result.MatchedCount > 0 && result.ModifiedCount > 0) _logger.LogInformation($"Knowledge base {id} has been updated");
            else _logger.LogWarning($"Couldn't update knowledge base {id}");

            return result.MatchedCount > 0 ? new KnowledgeBaseResource() { KnowledgeBase = knowledgeBase } : null;
        }

        private async Task<long> CountDocumentsByIdAsync(string id)
        {
            return await _knowledgeBaseRepository.CountAsync(kb => kb.Id == id);
        }
    }
}
