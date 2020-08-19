using System;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models.ValueTypes;

namespace Reasoning.Core.Models.Operators
{
    public class NotBetween : IOperator
    {
        public bool Compare(IValueType leftTerm, IValueType rightTerm)
        {
            if (!(leftTerm is BaseType)) throw new Exception($"Can't cast {leftTerm.GetType().Name} to BaseType");
            if (!(rightTerm is ListType)) throw new Exception($"Can't cast {rightTerm.GetType().Name} to ListType");

            return !((ListType)rightTerm).Between(leftTerm);
        }
    }
}
