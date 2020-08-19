using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class KnowledgeBase : IKnowledgeBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<IRule> RuleSet { get; set; }
    }
}
