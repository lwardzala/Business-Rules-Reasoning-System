using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Reasoning.Core.Contracts;

namespace Reasoning.MongoDb.Models
{
    public class ReasoningTask : IReasoningTask
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Description { get; set; }
        public string KnowledgeBaseId { get; set; }
        public ReasoningTaskStatus Status { get; set; }
        public ReasoningMethod ReasoningMethod { get; set; }
        public IVariable Hypothesis { get; set; }
        public IReasoningProcess ReasoningProcess { get; set; }
        public IEnumerable<IVariableSource> Sources { get; set; }
        public IEnumerable<IReasoningAction> Actions { get; set; }
    }
}
