using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class VariableSource : IVariableSource
    {
        public IEnumerable<string> VariableIds { get; set; }
        public IReasoningRequest Request { get; set; }
    }
}
