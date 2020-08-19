using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Test.Variable
{
    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void GetValue_WithNumberBaseType()
        {
            var variable = new Models.Variable("var1", 4);

            var result = variable.GetValue();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseType));
            Assert.AreEqual(4, ((BaseType)result).Value);
        }

        [TestMethod]
        public void GetValue_WithListType()
        {
            var variable = new Models.Variable("var1", new [] { 2, 3 });

            var result = variable.GetValue();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ListType));
            Assert.IsTrue(((ListType)result).Values.Count == 2);
        }
    }
}
