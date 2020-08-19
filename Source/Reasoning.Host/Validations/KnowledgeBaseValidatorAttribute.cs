using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Reasoning.Core.Contracts;

namespace Reasoning.Host.Validations
{
    public class KnowledgeBaseValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Knowledge Base can't be null");

            var knowledgeBase = (IKnowledgeBase)value;

            if (string.IsNullOrEmpty(knowledgeBase.Id))
                return new ValidationResult("Knowledge base `id` can't be null");

            if (knowledgeBase.RuleSet == null || knowledgeBase.RuleSet.Count == 0)
                return new ValidationResult("Knowledge base `ruleSet` must contain rules");

            if (knowledgeBase.RuleSet.Any(rule => rule.Conclusion == null))
                return new ValidationResult("All rules must contain `conclusion`");

            if (knowledgeBase.RuleSet.Any(rule => string.IsNullOrEmpty(rule.Conclusion.Id) || rule.Conclusion.Value == null))
                return new ValidationResult("All conclusions must contain `id` and `value`");

            if (knowledgeBase.RuleSet.Any(rule => rule.Predicates.Count == 0))
                return new ValidationResult("All rules must contain at least one predicate");

            if (knowledgeBase.RuleSet.Any(rule => rule.Predicates.Any(predicate => 
                predicate.RightTerm == null || predicate.LeftTerm == null)))
                return new ValidationResult("All predicates must contain `leftTerm` and `rightTerm`");

            if (knowledgeBase.RuleSet.Any(rule => rule.Predicates.Any(predicate => predicate.RightTerm.IsEmpty())))
                return new ValidationResult("All `rightTerm` properties must have `value`");

            if (knowledgeBase.RuleSet.Any(rule => rule.Predicates.Any(predicate => string.IsNullOrEmpty(predicate.LeftTerm.Id))))
                return new ValidationResult("All `leftTerm` properties must have `id`");

            return ValidationResult.Success;
        }
    }
}
