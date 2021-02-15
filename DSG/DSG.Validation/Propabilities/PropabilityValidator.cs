using DSG.Common;
using System.Windows.Controls;

namespace DSG.Validation.Propabilities
{
    public class PropabilityValidator : IPropabilityValidator
    {
        public IValidationMethods _validationMethods;

        public IValidationMethods ValidationMethods
        {
            get
            {
                Check.RequireInjected(_validationMethods, nameof(ValidationMethods), nameof(PropabilityValidator));
                return _validationMethods;
            }
            set { _validationMethods = value; }
        }

        public ValidationResult ValidatePropability(int value)
        {
            ValidationResult valueTooBigValidation = ValidationMethods.ValidateIntegerValueNotBigger(value, 100);

            if (valueTooBigValidation.IsValid == false)
            {
                return valueTooBigValidation;
            }

            ValidationResult valueTooSmallValidation = ValidationMethods.ValidateIntegerValueNotSmaller(value, 0);

            return valueTooSmallValidation;
        }
    }
}
