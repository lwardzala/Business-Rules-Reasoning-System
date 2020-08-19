using Reasoning.Core.Contracts;
using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Models.Operators
{
    public class LesserOrEqual : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            if (leftTerm.GetType() != typeof(BaseType) || rightTerm.GetType() != typeof(BaseType))
            {
                return false;
            }

            return (BaseType)leftTerm <= (BaseType)rightTerm;
        }
    }
}
