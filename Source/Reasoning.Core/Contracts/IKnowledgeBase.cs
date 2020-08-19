using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// Represents a collection of rules and predicates
    /// </summary>
    public interface IKnowledgeBase
    {
        /// <summary>
        /// Knowledge base unique id
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// Name of knowledge base. If null, inherits from Id
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Description of knowledge base
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Collection of rules
        /// </summary>
        IList<IRule> RuleSet { get; set; }
    }
}
