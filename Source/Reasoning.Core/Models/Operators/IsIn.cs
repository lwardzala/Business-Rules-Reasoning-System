using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models.Operators
{
    public class IsIn : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            return rightTerm.Contains(leftTerm);
        }
    }
}
