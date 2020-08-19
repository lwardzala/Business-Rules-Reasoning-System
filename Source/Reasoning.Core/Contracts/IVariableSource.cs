using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// An interface of HTTP resource for retrieving variables
    /// </summary>
    public interface IVariableSource
    {
        /// <summary>
        /// Variables possible to retrieve
        /// </summary>
        IEnumerable<string> VariableIds { get; set; }
        /// <summary>
        /// HTTP resource's request
        /// </summary>
        IReasoningRequest Request { get; set; }
    }
}
