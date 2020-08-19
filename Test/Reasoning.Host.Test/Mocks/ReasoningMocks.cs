using Reasoning.Core;
using Reasoning.Core.Contracts;
using Reasoning.Core.Models;
using Reasoning.MongoDb.Models;

namespace Reasoning.Host.Test.Mocks
{
    public static class ReasoningMocks
    {
        public static IReasoningProcess GetReasoningProcess()
        {
            return new ReasoningProcess
            {
                ReasoningMethod = ReasoningMethod.Deduction,
                KnowledgeBase = GetKnowledgeBase()
            };
        }

        public static IKnowledgeBase GetKnowledgeBase()
        {
            return new KnowledgeBaseBuilder()
                .SetId("knowledgeBase1")
                .SetName("Knowledge Base 1")
                .SetDescription("Testing reasoning service")
                .AddRule(new RuleBuilder()
                    .SetConclusion(new Variable("conclusion1", 1))
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var1", OperatorType.Equal, 3)
                        .SetLeftTermValue(3)
                        .Unwrap())
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var2", OperatorType.Equal, "OK")
                        .SetLeftTermValue("OK")
                        .Unwrap())
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var3", OperatorType.Equal, new[] {"opt1", "opt2"})
                        .SetLeftTermValue(new[] {"opt1", "opt2"})
                        .Unwrap())
                    .Unwrap())
                .AddRule(new RuleBuilder()
                    .SetConclusion(new Variable("conclusion2", 2))
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var1", OperatorType.Equal, 3)
                        .SetLeftTermValue(3)
                        .Unwrap())
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var2", OperatorType.Equal, "OK")
                        .SetLeftTermValue("OK")
                        .Unwrap())
                    .AddPredicate(new PredicateBuilder()
                        .ConfigurePredicate("var3", OperatorType.Equal, new[] {"opt1", "opt2"})
                        .SetLeftTermValue(new[] {"opt1"})
                        .Unwrap())
                    .Unwrap())
                .Unwrap();
        }

        public static IReasoningTask GetReasoningTask(ReasoningTaskStatus reasoningTaskStatus)
        {
            return new ReasoningTask
            {
                Id = "testId",
                Description = "test",
                KnowledgeBaseId = "testId",
                Status = reasoningTaskStatus,
                ReasoningMethod = ReasoningMethod.Deduction,
                ReasoningProcess = GetReasoningProcess()
            };
        }
    }
}
