using DSG.Validation;
using DSG.Validation.GenerationProfiles;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DSG.WinUI.Validation
{
    [ContentProperty(nameof(ComparisonValue))]
    public class ValidateGenerationProfile : ValidationRule
    {
        public ComparisonValue ComparisonValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string profileName = (string)value;

            GenerationProfileValidator validator = new GenerationProfileValidator();
            validator.ValidationMethods = new ValidationMethods();

            ValidationResult nameValidation = validator.ValidateName(profileName);

            if(nameValidation.IsValid == false)
            {
                return nameValidation;
            }

            IEnumerable<string> existingProfileNames = ComparisonValue.Value.Select(x => x.GenerationProfile.Name);
            ValidationResult duplicationValidation = validator.ValidateNameNoDuplicate(profileName, existingProfileNames);

            return duplicationValidation;
        }
    }
}
