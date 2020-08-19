using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Reasoning.MongoDb.Repositories
{
    public interface IReasoningRepository<TDocument>
    {
        Task<IList<TDocument>> GetAsync(Expression<Func<TDocument, bool>> filter, int batchSize = 100);
        Task<TDocument> GetAsync(string id);
        void Create(TDocument document);
        Task<ReplaceOneResult> UpdateAsync(string id, TDocument document);
        void Update(string id, TDocument document);
        Task<DeleteResult> RemoveAsync(TDocument document);
        Task<DeleteResult> RemoveAsync(string id);
        Task<long> CountAsync(Expression<Func<TDocument, bool>> filter);
    }
}
