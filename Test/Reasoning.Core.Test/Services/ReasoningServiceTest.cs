using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models;
using Reasoning.Core.Services;

namespace Reasoning.Core.Test.Services
{
    [TestClass]
    public class ReasoningServiceTest
    {
        [TestMethod]
        public void ContinueReasoning_Deduction_OneTrue()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);

            var result = new ReasoningService().ContinueReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.FINISHED, result.State);
            Assert.AreEqual(EvaluationMessage.PASSED, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 1);
            Assert.AreEqual(conclusion1, result.ReasonedItems[0]);
        }

        [TestMethod]
        public void ContinueReasoning_Deduction_NoTrue()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);
            reasoningProcess.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.Value = 17;

            var result = new ReasoningService().ContinueReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.FINISHED, result.State);
            Assert.AreEqual(EvaluationMessage.FAILED, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 0);
        }

        [TestMethod]
        public void ContinueReasoning_Hypothesis_TrueHypothesis()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.HypothesisTesting, conclusion1, conclusion2);
            reasoningProcess.Hypothesis = conclusion1;

            var result = new ReasoningService().ContinueReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.FINISHED, result.State);
            Assert.AreEqual(EvaluationMessage.PASSED, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 1);
            Assert.AreEqual(result.Hypothesis, result.ReasonedItems[0]);
        }

        [TestMethod]
        public void ContinueReasoning_Hypothesis_FalseHypothesis()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.HypothesisTesting, conclusion1, conclusion2);
            reasoningProcess.Hypothesis = conclusion2;

            var result = new ReasoningService().ContinueReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.FINISHED, result.State);
            Assert.AreEqual(EvaluationMessage.FAILED, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 0);
        }

        [TestMethod]
        public void StartReasoning_Deduction()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);

            var result = new ReasoningService().StartReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.STOPPED, result.State);
            Assert.AreEqual(EvaluationMessage.MISSING_VALUES, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 0);
        }

        [TestMethod]
        public void ClearReasoning()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);

            var result = new ReasoningService().ClearReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.INITIALIZED, result.State);
            Assert.AreEqual(EvaluationMessage.NONE, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 0);
            Assert.IsTrue(result.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsTrue(result.KnowledgeBase.RuleSet[0].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsTrue(result.KnowledgeBase.RuleSet[0].Predicates[2].LeftTerm.IsEmpty());
            Assert.IsTrue(result.KnowledgeBase.RuleSet[1].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsTrue(result.KnowledgeBase.RuleSet[1].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsTrue(result.KnowledgeBase.RuleSet[1].Predicates[2].LeftTerm.IsEmpty());
        }

        [TestMethod]
        public void ResetReasoning()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);

            var result = new ReasoningService().ResetReasoning(reasoningProcess);

            Assert.AreEqual(ReasoningState.INITIALIZED, result.State);
            Assert.AreEqual(EvaluationMessage.NONE, result.EvaluationMessage);
            Assert.IsTrue(result.ReasonedItems.Count == 0);
            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[2].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[2].LeftTerm.IsEmpty());
        }

        [TestMethod]
        public void GetAllMissingVariableIds()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var service = new ReasoningService();

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);
            reasoningProcess = service.ClearReasoning(reasoningProcess);

            var result = service.GetAllMissingVariableIds(reasoningProcess);

            var enumerable = result.ToList();
            Assert.IsTrue(enumerable.Count == 3);
            Assert.AreEqual("var1", enumerable[0]);
            Assert.AreEqual("var2", enumerable[1]);
            Assert.AreEqual("var3", enumerable[2]);
        }

        [TestMethod]
        public void SetValues()
        {
            var conclusion1 = new Models.Variable("conclusion1", "conclusion1", "Should pass");
            var conclusion2 = new Models.Variable("conclusion2", "conclusion2", "Shouldn't pass");

            var service = new ReasoningService();

            var reasoningProcess = MockReasoningProcess(ReasoningMethod.Deduction, conclusion1, conclusion2);
            reasoningProcess = service.ClearReasoning(reasoningProcess);

            var result = service.SetValues(reasoningProcess, new List<IVariable>
            {
                new Models.Variable("var1", 3),
                new Models.Variable("var2", null, "OK"),
                new Models.Variable("var3", new [] { "opt1" })
            });

            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[0].Predicates[2].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[0].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[1].LeftTerm.IsEmpty());
            Assert.IsFalse(result.KnowledgeBase.RuleSet[1].Predicates[2].LeftTerm.IsEmpty());
        }

        private IReasoningProcess MockReasoningProcess(ReasoningMethod reasoningMethod, IVariable conclusion1, IVariable conclusion2)
        {
            return new ReasoningProcess
            {
                ReasoningMethod = reasoningMethod,
                KnowledgeBase = new KnowledgeBaseBuilder()
                    .SetId("knowledgeBase1")
                    .SetName("Knowledge Base 1")
                    .SetDescription("Testing reasoning service")
                    .AddRule(new RuleBuilder()
                        .SetConclusion(conclusion1)
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var1", OperatorType.Equal, 3)
                            .SetLeftTermValue(3)
                            .Unwrap())
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var2", OperatorType.Equal, "OK")
                            .SetLeftTermValue("OK")
                            .Unwrap())
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var3", OperatorType.Equal, new [] { "opt1", "opt2" })
                            .SetLeftTermValue(new [] { "opt1", "opt2" })
                            .Unwrap())
                        .Unwrap())
                    .AddRule(new RuleBuilder()
                        .SetConclusion(conclusion2)
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var1", OperatorType.Equal, 3)
                            .SetLeftTermValue(3)
                            .Unwrap())
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var2", OperatorType.Equal, "OK")
                            .SetLeftTermValue("OK")
                            .Unwrap())
                        .AddPredicate(new PredicateBuilder()
                            .ConfigurePredicate("var3", OperatorType.Equal, new [] { "opt1", "opt2" })
                            .SetLeftTermValue(new [] { "opt1" })
                            .Unwrap())
                        .Unwrap())
                    .Unwrap()
            };
        }
    }
}
