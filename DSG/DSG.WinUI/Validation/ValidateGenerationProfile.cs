using DSG.Validation;
using DSG.Validation.GenerationProfiles;
using System.Globalization;
using System.Windows.Controls;

namespace DSG.WinUI.Validation
{
    public class ValidateGenerationProfile : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string profileName = (string)value;

            GenerationProfileValidator validator = new GenerationProfileValidator();
            validator.ValidationMethods = new ValidationMethods();

            return validator.ValidateName(profileName);
        }
    }
}
