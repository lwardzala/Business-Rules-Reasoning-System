using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Test.Operator
{
    [TestClass]
    public class BaseTypeOperatorsTests
    {
        [TestMethod]
        public void Equal_TwoSameNumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.Equal, 3)
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void NotEqual_TwoDifferentNumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.NotEqual, 3)
                .SetLeftTermValue(4)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void GreaterOrEqual_TwoSameNumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.GreaterOrEqual, 3)
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void GreaterOrEqual_OneGreater_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.GreaterOrEqual, 3)
                .SetLeftTermValue(4)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void GreaterOrEqual_OneGreater_ReturnsFalse()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.GreaterOrEqual, 3)
                .SetLeftTermValue(2)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsFalse(predicate.Result);
        }

        [TestMethod]
        public void LesserOrEqual_TwoSameNumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.LesserOrEqual, 3)
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void LesserOrEqual_OneGreater_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.LesserOrEqual, 3)
                .SetLeftTermValue(2)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void LesserOrEqual_OneGreater_ReturnsFalse()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.LesserOrEqual, 3)
                .SetLeftTermValue(4)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsFalse(predicate.Result);
        }

        [TestMethod]
        public void GreaterThan_TwoSameNumberValues_ReturnsFalse()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.GreaterThan, 3)
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsFalse(predicate.Result);
        }

        [TestMethod]
        public void GreaterThan_OneGreater_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.GreaterThan, 3)
                .SetLeftTermValue(4)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void LesserThan_TwoSameNumberValues_ReturnsFalse()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.LesserThan, 3)
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsFalse(predicate.Result);
        }

        [TestMethod]
        public void LesserThan_OneGreater_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.LesserThan, 3)
                .SetLeftTermValue(2)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void Between_NumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.Between, new [] { 3, 7 })
                .SetLeftTermValue(4)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void NotBetween_NumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.NotBetween, new[] { 3, 7 })
                .SetLeftTermValue(2)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void IsIn_NumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.IsIn, new[] { 3, 7, 8 })
                .SetLeftTermValue(3)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }

        [TestMethod]
        public void NotIn_NumberValues_ReturnsTrue()
        {
            var predicate = new PredicateBuilder()
                .ConfigurePredicate("var1", OperatorType.NotIn, new[] { 3, 7, 8 })
                .SetLeftTermValue(2)
                .Unwrap();

            predicate.Evaluate();

            Assert.IsTrue(predicate.Evaluated);
            Assert.IsTrue(predicate.Result);
        }
    }
}
