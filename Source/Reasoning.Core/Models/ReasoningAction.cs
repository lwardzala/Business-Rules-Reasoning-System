using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class ReasoningAction : IReasoningAction
    {
        public IEnumerable<IVariable> ReasoningItems { get; set; }
        public IEnumerable<IReasoningRequest> ReasoningRequests { get; set; }
    }
}
