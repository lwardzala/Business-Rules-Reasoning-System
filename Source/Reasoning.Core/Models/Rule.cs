using System.Linq;
using System.Collections.Generic;

using Reasoning.Core.Contracts;

namespace Reasoning.Core.Models
{
    public class Rule : IRule
    {
        public IVariable Conclusion { get; set; }
        public IList<IPredicate> Predicates { get; set; }
        public bool Result { get; set; }
        public bool Evaluated { get; set; }

        public void Evaluate()
        {
            if (Evaluated) return;

            foreach (var predicate in Predicates)
            {
                if (predicate.GetMissingVariableId() != null) continue;

                predicate.Evaluate();

                if (predicate.Result) continue;

                Result = false;
                Evaluated = true;
                return;
            }

            if (Predicates.Any(_ => _.Evaluated && !_.Result) || Predicates.All(_ => _.Evaluated))
            {
                Result = true;
                Evaluated = true;
            }
        }

        public bool IsValid()
        {
            return Predicates.All(predicate => predicate.GetMissingVariableId() == null);
        }
    }
}
