using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models.Operators
{
    public class Equal : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            return leftTerm.Equals(rightTerm);
        }
    }
}
