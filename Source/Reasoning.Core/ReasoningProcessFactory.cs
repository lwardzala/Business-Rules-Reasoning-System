using Reasoning.Core.Contracts;
using Reasoning.Core.Models;

namespace Reasoning.Core
{
    /// <summary>
    /// Factory of Reasoning Process
    /// </summary>
    public static class ReasoningProcessFactory
    {
        /// <summary>
        /// Creates instance of Reasoning Process
        /// </summary>
        /// <param name="knowledgeBase">Knowledge Base object</param>
        /// <param name="reasoningMethod">Reasoning Method</param>
        /// <param name="hypothesis">Hypothesis for Hypothesis testing method</param>
        /// <returns>Instance of Reasoning Process</returns>
        public static IReasoningProcess CreateInstance(
            IKnowledgeBase knowledgeBase,
            ReasoningMethod reasoningMethod,
            IVariable hypothesis
            )
        {
            return new ReasoningProcess
            {
                KnowledgeBase = knowledgeBase,
                ReasoningMethod = reasoningMethod,
                Hypothesis = hypothesis
            };
        }
    }
}
