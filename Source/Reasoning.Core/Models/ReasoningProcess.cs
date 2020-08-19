using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class ReasoningProcess : IReasoningProcess
    {
        public ReasoningMethod ReasoningMethod { get; set; }
        public IKnowledgeBase KnowledgeBase { get; set; }
        public ReasoningState State { get; set; }
        public IList<IVariable> ReasonedItems { get; set; }
        public EvaluationMessage EvaluationMessage { get; set; }
        public IVariable Hypothesis { get; set; }

        public ReasoningProcess()
        {
            State = ReasoningState.INITIALIZED;
            EvaluationMessage = EvaluationMessage.NONE;
            ReasonedItems = new List<IVariable>();
        }
    }
}
