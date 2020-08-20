using System.Collections.Generic;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models;

namespace Reasoning.Core
{
    public class KnowledgeBaseBuilder
    {
        private readonly IKnowledgeBase _knowledgeBase;

        public KnowledgeBaseBuilder()
        {
            _knowledgeBase = new KnowledgeBase
            {
                RuleSet = new List<IRule>()
            };
        }

        public KnowledgeBaseBuilder SetId(string id)
        {
            _knowledgeBase.Id = id;
            return this;
        }

        public KnowledgeBaseBuilder SetName(string name)
        {
            _knowledgeBase.Name = name;
            return this;
        }

        public KnowledgeBaseBuilder SetDescription(string description)
        {
            _knowledgeBase.Description = description;
            return this;
        }

        public KnowledgeBaseBuilder AddRule(IRule rule)
        {
            _knowledgeBase.RuleSet.Add(rule);
            return this;
        }

        public IKnowledgeBase Unwrap() => _knowledgeBase;
    }

    public class RuleBuilder
    {
        private readonly IRule _rule;

        public RuleBuilder()
        {
            _rule = new Rule
            {
                Predicates = new List<IPredicate>(),
                Result = false,
                Evaluated = false
            };
        }

        public RuleBuilder SetConclusion(IVariable ruleConclusion)
        {
            _rule.Conclusion = ruleConclusion;
            return this;
        }

        public RuleBuilder AddPredicate(IPredicate predicate)
        {
            _rule.Predicates.Add(predicate);
            return this;
        }

        public IRule Unwrap() => _rule;
    }

    public class PredicateBuilder
    {
        private readonly IPredicate _predicate;

        public PredicateBuilder()
        {
            _predicate = new Predicate
            {
                LeftTerm = new Variable(),
                RightTerm = new Variable(),
                Result = false,
                Evaluated = false
            };
        }

        public PredicateBuilder ConfigurePredicate(string variableId, OperatorType operatorType, object rightTermValue)
        {
            _predicate.LeftTerm.Id = variableId;
            _predicate.LeftTerm.Name = variableId;
            _predicate.RightTerm.Id = variableId;
            _predicate.RightTerm.Name = variableId;
            _predicate.Operator = operatorType;
            _predicate.RightTerm.Value = rightTermValue;
            return this;
        }

        public PredicateBuilder ConfigurePredicate(string variableId, string variableName, OperatorType operatorType, object rightTermValue)
        {
            _predicate.LeftTerm.Id = variableId;
            _predicate.LeftTerm.Name = variableName;
            _predicate.RightTerm.Id = variableId;
            _predicate.RightTerm.Name = variableName;
            _predicate.Operator = operatorType;
            _predicate.RightTerm.Value = rightTermValue;
            return this;
        }

        public PredicateBuilder SetLeftTermValue(object leftTermValue)
        {
            _predicate.LeftTerm.Value = leftTermValue;
            return this;
        }

        public IPredicate Unwrap() => _predicate;
    }

    public class VariableBuilder
    {
        private readonly IVariable _variable;

        public VariableBuilder()
        {
            _variable = new Variable();
        }

        public VariableBuilder SetId(string id)
        {
            _variable.Id = id;
            return this;
        }

        public VariableBuilder SetName(string name)
        {
            _variable.Name = name;
            return this;
        }

        public VariableBuilder SetValue(object value)
        {
            _variable.Value = value;
            return this;
        }

        public IVariable Unwrap() => _variable;
    }
}
