using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Test.Rule
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void Evaluate_AllTruePredicates()
        {
            var rule = new RuleBuilder()
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var1", OperatorType.Equal, 4)
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.Between, new [] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.NotEqual, new[] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .Unwrap();

            rule.Evaluate();

            Assert.IsTrue(rule.Evaluated);
            Assert.IsTrue(rule.Result);
        }

        [TestMethod]
        public void Evaluate_OneFalsePredicate()
        {
            var rule = new RuleBuilder()
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var1", OperatorType.Equal, 4)
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.Between, new[] { 3, 7 })
                    .SetLeftTermValue(10)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.NotEqual, new[] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .Unwrap();

            rule.Evaluate();

            Assert.IsTrue(rule.Evaluated);
            Assert.IsFalse(rule.Result);
        }

        [TestMethod]
        public void Evaluate_OneAlreadyTruePredicate()
        {
            var rule = new RuleBuilder()
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var1", OperatorType.Equal, 4)
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.Between, new[] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.NotEqual, new[] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .Unwrap();

            rule.Predicates[0].Evaluated = true;
            rule.Predicates[0].Result = true;

            rule.Evaluate();

            Assert.IsTrue(rule.Evaluated);
            Assert.IsTrue(rule.Result);
        }

        [TestMethod]
        public void Evaluate_OneAlreadyFalsePredicate()
        {
            var rule = new RuleBuilder()
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var1", OperatorType.Equal, 4)
                    .SetLeftTermValue(4)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.Between, new[] { 3, 7 })
                    .SetLeftTermValue(10)
                    .Unwrap())
                .AddPredicate(new PredicateBuilder()
                    .ConfigurePredicate("var2", OperatorType.NotEqual, new[] { 3, 7 })
                    .SetLeftTermValue(4)
                    .Unwrap())
                .Unwrap();

            rule.Predicates[0].Evaluated = true;
            rule.Predicates[0].Result = false;

            rule.Evaluate();

            Assert.IsTrue(rule.Evaluated);
            Assert.IsFalse(rule.Result);
        }
    }
}
