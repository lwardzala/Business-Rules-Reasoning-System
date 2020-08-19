using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Host.Resources
{
    public class ReasoningTaskResource
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string KnowledgeBaseId { get; set; }
        public ReasoningMethod ReasoningMethod { get; set; }
        public ReasoningTaskStatus Status { get; set; }
        public EvaluationMessage EvaluationMessage { get; set; }
        public IEnumerable<string> MissingVariableIds { get; set; }
        public IList<IVariable> ReasonedItems { get; set; }
        public IEnumerable<IVariableSource> Sources { get; set; }
        public IEnumerable<IReasoningAction> Actions { get; set; }
    }
}
