using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

using Reasoning.Core.Contracts;
using Reasoning.MongoDb.Configuration;

namespace Reasoning.MongoDb.Repositories
{
    public class KnowledgeBaseRepository : RepositoryBase<IKnowledgeBase>, IKnowledgeBaseRepository
    {
        public KnowledgeBaseRepository(IMongoDatabaseSettings settings) : base(settings, settings.KnowledgeBaseCollectionName)
        {
        }

        public Task<long> CountAsync(Expression<Func<IKnowledgeBase, bool>> filter) =>
            collection.CountDocumentsAsync(filter);

        public void Create(IKnowledgeBase document) =>
            collection.InsertOneAsync(document);

        public async Task<IList<IKnowledgeBase>> GetAsync(Expression<Func<IKnowledgeBase, bool>> filter, int batchSize = 100)
        {
            return await collection.Find(filter, new FindOptions() { BatchSize = batchSize }).ToListAsync();
        }

        public Task<IKnowledgeBase> GetAsync(string id) =>
            collection.Find(doc => doc.Id == id).FirstOrDefaultAsync();

        public Task<DeleteResult> RemoveAsync(IKnowledgeBase document) =>
            collection.DeleteOneAsync(doc => doc.Id == document.Id);

        public Task<DeleteResult> RemoveAsync(string id) =>
            collection.DeleteOneAsync(doc => doc.Id == id);

        public void Update(string id, IKnowledgeBase document) =>
            collection.ReplaceOne(doc => doc.Id == id, document);

        public Task<ReplaceOneResult> UpdateAsync(string id, IKnowledgeBase document) =>
            collection.ReplaceOneAsync(doc => doc.Id == id, document);
    }
}
