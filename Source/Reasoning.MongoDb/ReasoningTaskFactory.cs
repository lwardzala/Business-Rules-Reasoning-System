using System.Collections.Generic;

using Reasoning.Core.Contracts;
using Reasoning.MongoDb.Models;

namespace Reasoning.MongoDb
{
    public static class ReasoningTaskFactory
    {
        public static IReasoningTask CreateInstance(
            IReasoningProcess reasoningProcess,
            string description,
            IEnumerable<IVariableSource> sources,
            IEnumerable<IReasoningAction> actions
            )
        {
            return new ReasoningTask
            {
                Description = description,
                KnowledgeBaseId = reasoningProcess.KnowledgeBase.Id,
                ReasoningMethod = reasoningProcess.ReasoningMethod,
                Hypothesis = reasoningProcess.Hypothesis,
                Sources = sources,
                Actions = actions,
                Status = ReasoningTaskStatus.WAITING,
                ReasoningProcess = reasoningProcess
            };
        }
    }
}
