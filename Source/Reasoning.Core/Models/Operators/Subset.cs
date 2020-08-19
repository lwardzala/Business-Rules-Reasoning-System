using Reasoning.Core.Contracts;
using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Models.Operators
{
    public class Subset : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            switch (rightTerm)
            {
                case ListType type:
                    return type.Contains(leftTerm);
                case BaseType type:
                    return leftTerm is BaseType && type.Equals(leftTerm);
                default:
                    return false;
            }
        }
    }
}
