using System.ComponentModel.DataAnnotations;

using Reasoning.Core.Contracts;

namespace Reasoning.Host.Validations
{
    public class HypothesisValidationAttribute : ValidationAttribute
    {
        public string GetError() => "Hypothesis must contain `id` and `value`";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var hypothesis = (IVariable) value;

            if (string.IsNullOrEmpty(hypothesis.Id) || hypothesis.Value == null)
                return new ValidationResult(GetError());

            return ValidationResult.Success;
        }
    }
}
