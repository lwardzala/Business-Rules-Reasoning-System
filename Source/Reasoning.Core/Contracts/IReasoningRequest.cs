using System.Collections.Generic;

namespace Reasoning.Core.Contracts
{
    /// <summary>
    /// Specifies possible HTTP methods
    /// </summary>
    public enum ReasoningRequestMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    /// <summary>
    /// An infterface for reasoning request item
    /// </summary>
    public interface IReasoningRequest
    {
        /// <summary>
        /// HTTP method
        /// </summary>
        ReasoningRequestMethod Method { get; set; }
        /// <summary>
        /// Full uri string
        /// </summary>
        string Uri { get; set; }
        /// <summary>
        /// HTTP Headers
        /// </summary>
        IDictionary<string, string> Headers { get; set; }
        /// <summary>
        /// Request content. Only JSON structures
        /// </summary>
        string Body { get; set; }
    }
}
