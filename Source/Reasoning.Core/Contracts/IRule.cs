using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface of reasoning rule
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Rule conclusion. Goes to reasoning result items if evaluated as true
        /// </summary>
        IVariable Conclusion { get; set; }
        /// <summary>
        /// Collection of rule's predicates
        /// </summary>
        IList<IPredicate> Predicates { get; set; }
        /// <summary>
        /// Rule's evaluation result
        /// </summary>
        bool Result { get; set; }
        /// <summary>
        /// Specifies if rule has been evaluated
        /// </summary>
        bool Evaluated { get; set; }

        /// <summary>
        /// Checks if rule is ready to be evaluated
        /// </summary>
        /// <returns>True or False</returns>
        bool IsValid();
        /// <summary>
        /// Runs rule evaluation
        /// </summary>
        void Evaluate();
    }
}
