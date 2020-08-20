using Reasoning.Core;
using Reasoning.Core.Contracts;
using Reasoning.MongoDb.Models;

namespace Reasoning.Host.Test.Mocks
{
    public static class ReasoningMocks
    {
        public static IReasoningProcess GetReasoningProcess()
        {
            return ReasoningProcessFactory.CreateInstance(
                GetKnowledgeBase(),
                ReasoningMethod.Deduction,
                null);
        }
        
        public static IKnowledgeBase GetKnowledgeBase()
        {
            return new KnowledgeBaseBuilder()
                .SetId("knowledgeBase1")
                .SetName("Knowledge Base 1")
                .SetDescription("Testing reasoning service")
                .AddRule(new RuleBuilder()
                    .SetConclusion(new VariableBuilder()
                        .SetId("conclusion1")
                        .SetValue(1)
                        .Unwrap())
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
                    .SetConclusion(new VariableBuilder()
                        .SetId("conclusion2")
                        .SetValue(2)
                        .Unwrap())
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
