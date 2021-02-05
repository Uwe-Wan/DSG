using System.Windows.Controls;

namespace DSG.Validation
{
    public class ValidationMethods : IValidationMethods
    {
        public ValidationResult ValidateStringLength(string name, int length)
        {
            if (name.Length > length)
            {
                return new ValidationResult(false, "Name must not be longer than 20 characters.");
            }

            return ValidationResult.ValidResult;
        }

        public ValidationResult ValidateStringNotNullNotEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(false, "Name must be set.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
