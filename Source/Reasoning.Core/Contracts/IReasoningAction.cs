using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface for Reasoning Action
    /// </summary>
    public interface IReasoningAction
    {
        /// <summary>
        /// Possible results of reasoning
        /// </summary>
        IEnumerable<IVariable> ReasoningItems { get; set; }
        /// <summary>
        /// Requests to be launched after reasoning
        /// </summary>
        IEnumerable<IReasoningRequest> ReasoningRequests { get; set; }
    }
}
