using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Reasoning.Core.Contracts;
using Reasoning.Host.Validations;

namespace Reasoning.Host.Resources
{
    public class CreateReasoningTaskResource
    {
        public string Description { get; set; }
        [Required]
        public string KnowledgeBaseId { get; set; }
        [Required]
        public ReasoningMethod ReasoningMethod { get; set; }
        [HypothesisValidation]
        public IVariable Hypothesis { get; set; }
        public IEnumerable<IVariableSource> Sources { get; set; }
        public IEnumerable<IReasoningAction> Actions { get; set; }
    }
}
