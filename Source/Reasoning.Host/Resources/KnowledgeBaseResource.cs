using Reasoning.Core.Contracts;
using Reasoning.Host.Validations;

namespace Reasoning.Host.Resources
{
    public class KnowledgeBaseResource
    {
        [KnowledgeBaseValidator]
        public IKnowledgeBase KnowledgeBase { get; set; }
    }
}
