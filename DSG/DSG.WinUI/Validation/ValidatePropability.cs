using DSG.Validation;
using DSG.Validation.Propabilities;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DSG.WinUI.Validation
{
    [ContentProperty(nameof(ComparisonValue))]
    public class ValidatePropability : ValidationRule
    {
        public ComparisonValueForPropability ComparisonValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString();

            ValidationMethods validationMethods = new ValidationMethods();
            ValidationResult convertibleToIntegerResult = validationMethods.ValidateConvertibleToInteger(input);

            if(convertibleToIntegerResult.IsValid == false)
            {
                return convertibleToIntegerResult;
            }

            int propability = int.Parse(input);

            PropabilityValidator validator = new PropabilityValidator();
            validator.ValidationMethods = validationMethods;

            return validator.ValidatePropability(propability);
        }
    }
}
