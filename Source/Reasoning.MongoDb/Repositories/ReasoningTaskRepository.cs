using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

using Reasoning.Core.Contracts;
using Reasoning.MongoDb.Configuration;
using Reasoning.MongoDb.Models;

namespace Reasoning.MongoDb.Repositories
{
    public class ReasoningTaskRepository : RepositoryBase<ReasoningTask>, IReasoningTaskRepository
    {
        public ReasoningTaskRepository(IMongoDatabaseSettings settings) : base(settings, settings.ReasoningTaskCollectionName)
        {
        }

        public async Task<long> CountAsync(Expression<Func<IReasoningTask, bool>> filter)
        {
            var filterConverted = Expression.Lambda<Func<ReasoningTask, bool>>(filter.Body, filter.Parameters);

            return await collection.CountDocumentsAsync(filterConverted);
        }

        public void Create(IReasoningTask document) =>
            collection.InsertOneAsync((ReasoningTask)document);

        public async Task<IList<IReasoningTask>> GetAsync(Expression<Func<IReasoningTask, bool>> filter, int batchSize = 100)
        {
            var filterConverted = Expression.Lambda<Func<ReasoningTask, bool>>(filter.Body, filter.Parameters);
            
            return (await collection.Find(filterConverted, new FindOptions() { BatchSize = batchSize }).ToListAsync()).ToList<IReasoningTask>();
        }

        public async Task<IReasoningTask> GetAsync(string id)
        {
            return await collection.Find(doc => doc.Id == id).FirstOrDefaultAsync();
        }

        public Task<DeleteResult> RemoveAsync(IReasoningTask document) =>
            collection.DeleteOneAsync(doc => doc.Id == document.Id);

        public Task<DeleteResult> RemoveAsync(string id) =>
            collection.DeleteOneAsync(doc => doc.Id == id);

        public void Update(string id, IReasoningTask document) =>
            collection.ReplaceOne(doc => doc.Id == id, (ReasoningTask)document);

        public Task<ReplaceOneResult> UpdateAsync(string id, IReasoningTask document) =>
            collection.ReplaceOneAsync(doc => doc.Id == id, (ReasoningTask)document);
    }
}
