using MongoDB.Driver;

using Reasoning.MongoDb.Configuration;

namespace Reasoning.MongoDb.Repositories
{
    public abstract class RepositoryBase<TDocument>
    {
        protected readonly IMongoCollection<TDocument> collection;

        public RepositoryBase(IMongoDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            collection = database.GetCollection<TDocument>(collectionName);
        }
    }
}
