using System;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class Predicate : IPredicate
    {
        public IVariable LeftTerm { get; set; }
        public IVariable RightTerm { get; set; }
        public OperatorType Operator { get; set; }
        public bool Result { get; set; }
        public bool Evaluated { get; set; }

        public void Evaluate()
        {
            if (!IsValid()) throw new Exception($"Evaluation of predicate has failed. Missing value {LeftTerm.Id}.");
            if (Evaluated) return;

            try
            {
                var operatorType = Type.GetType($"Reasoning.Core.Models.Operators.{Enum.GetName(typeof(OperatorType), Operator)}, Reasoning.Core");
                var operatorInstance = (IOperator)Activator.CreateInstance(operatorType);
                Result = operatorInstance.Compare(LeftTerm.GetValue(), RightTerm.GetValue());
                Evaluated = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unknown operator instance of predicate", ex);
            }
        }

        public string GetMissingVariableId()
        {
            return LeftTerm.IsEmpty() ? LeftTerm.Id : null;
        }

        public bool IsValid()
        {
            return !LeftTerm.IsEmpty() && !RightTerm.IsEmpty();
        }
    }
}
