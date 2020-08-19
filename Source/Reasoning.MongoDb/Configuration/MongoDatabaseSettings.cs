namespace Reasoning.MongoDb.Configuration
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string KnowledgeBaseCollectionName { get; set; }
        public string ReasoningTaskCollectionName { get; set; }
        public string VariableSourceCollectionName { get; set; }
        public string VariableCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
