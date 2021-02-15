using DSG.Common;
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
                return new ValidationResult(false, Text.NameTooLong);
            }

            return ValidationResult.ValidResult;
        }

        public ValidationResult ValidateStringNotNullNotEmpty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult(false, Text.NameNotSet);
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

        public ValidationResult ValidateIntegerValueNotBigger(int toValidate, int upperBoarder)
        {
            if (toValidate > upperBoarder)
            {
                return new ValidationResult(false, Text.IntToBig + upperBoarder);
            }

            return ValidationResult.ValidResult;
        }

        public ValidationResult ValidateIntegerValueNotSmaller(int toValidate, int lowerBoarder)
        {
            if (toValidate < lowerBoarder)
            {
                return new ValidationResult(false, Text.IntToSmall + lowerBoarder);
            }

            return ValidationResult.ValidResult;
        }

        public ValidationResult ValidateConvertibleToInteger(string toValidate)
        {
            if (int.TryParse(toValidate, out int result) == false)
            {
                return new ValidationResult(false, Text.NotConvertibleToInt);
            }

            return ValidationResult.ValidResult;
        }
    }
}
