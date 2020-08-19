using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// Specifies reasoning task status
    /// </summary>
    public enum ReasoningTaskStatus
    {
        WAITING,
        STOPPED,
        FINISHED,
        CANCELLED
    }

    /// <summary>
    /// An interface for reasoning task. Contains all data for reasoning processing
    /// </summary>
    public interface IReasoningTask
    {
        /// <summary>
        /// Reasoning task id
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// Reasoning description
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Used knowledge base id
        /// </summary>
        string KnowledgeBaseId { get; set; }
        /// <summary>
        /// Task status
        /// </summary>
        ReasoningTaskStatus Status { get; set; }
        /// <summary>
        /// Used reasoning method
        /// </summary>
        ReasoningMethod ReasoningMethod { get; set; }
        /// <summary>
        /// Passed hypothesis
        /// </summary>
        IVariable Hypothesis { get; set; }
        /// <summary>
        /// Current reasoning process
        /// </summary>
        IReasoningProcess ReasoningProcess { get; set; }
        /// <summary>
        /// Collection of HTTP resources for retrieving variable values
        /// </summary>
        IEnumerable<IVariableSource> Sources { get; set; }
        /// <summary>
        /// Collection of reasoning actions
        /// </summary>
        IEnumerable<IReasoningAction> Actions { get; set; }
    }
}
