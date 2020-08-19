using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Test.Operator
{
    [TestClass]
    public class ListTypeOperatorsTests
    {
        [TestMethod]
        public void Equal_SameNumberArrays_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.Equal, new [] { 2, 3 })
                .SetLeftTermValue(new [] { 2, 3 })
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void NotEqual_DifferentNumberArrays_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.NotEqual, new[] { 2, 3 })
                .SetLeftTermValue(new[] { 2, 3, 4 })
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void Contains_SameNumberArrays_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.IsIn, new[] { 2, 3, 4 })
                .SetLeftTermValue(new[] { 2, 3 })
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void NotContains_DifferentNumberArrays_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.NotIn, new[] { 2, 3, 6 })
                .SetLeftTermValue(new[] { 1, 3, 4 })
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }
    }
}
