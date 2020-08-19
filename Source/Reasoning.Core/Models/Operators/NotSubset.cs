using Reasoning.Core.Contracts;
using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Models.Operators
{
    public class NotSubset : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            switch (rightTerm)
            {
                case ListType type:
                    return !type.Contains(leftTerm);
                case BaseType type when leftTerm is BaseType:
                    return !type.Equals(leftTerm);
                case BaseType _:
                    return true;
                default:
                    return true;
            }
        }
    }
}
