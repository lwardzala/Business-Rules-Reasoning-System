namespace Reasoning.MongoDb.Configuration
{
    public interface IMongoDatabaseSettings
    {
        string KnowledgeBaseCollectionName { get; set; }
        string ReasoningTaskCollectionName { get; set; }
        string VariableSourceCollectionName { get; set; }
        string VariableCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
