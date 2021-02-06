using System.Collections.Generic;
using System.Linq;
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

        public ValidationResult ValidateNameNoDuplicate(string name, IEnumerable<string> existingNames, string propertyName)
        {
            if (existingNames.Contains(name))
            {
                return new ValidationResult(false, $"There exists a {propertyName} with that name.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
