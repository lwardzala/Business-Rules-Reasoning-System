using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class ReasoningRequest : IReasoningRequest
    {
        public ReasoningRequestMethod Method { get; set; }
        public string Uri { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
