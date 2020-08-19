using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// Specifies reasoning evaluation state
    /// </summary>
    public enum ReasoningState
    {
        INITIALIZED,
        STARTED,
        STOPPED,
        FINISHED
    }

    /// <summary>
    /// Specifies evaluation result
    /// </summary>
    public enum EvaluationMessage
    {
        NONE,
        PASSED,
        FAILED,
        ERROR,
        MISSING_VALUES
    }

    /// <summary>
    /// Specifies reasoning method
    /// </summary>
    public enum ReasoningMethod
    {
        Deduction,
        HypothesisTesting
    }

    /// <summary>
    /// An interface of reasoning state
    /// </summary>
    public interface IReasoningProcess
    {
        /// <summary>
        /// Applied reasoning method
        /// </summary>
        ReasoningMethod ReasoningMethod { get; set; }
        /// <summary>
        /// Current knowledge base state
        /// </summary>
        IKnowledgeBase KnowledgeBase { get; set; }
        /// <summary>
        /// Reasoning evaluation state
        /// </summary>
        ReasoningState State { get; set; }
        /// <summary>
        /// Collection of reasoning results
        /// </summary>
        IList<IVariable> ReasonedItems { get; set; }
        /// <summary>
        /// Evaluation result state
        /// </summary>
        EvaluationMessage EvaluationMessage { get; set; }
        /// <summary>
        /// Hypothesis in Hypothesis Testing method
        /// </summary>
        IVariable Hypothesis { get; set; }
    }
}
