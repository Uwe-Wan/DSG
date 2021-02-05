using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using System.Windows.Controls;

namespace DSG.Validation.GenerationProfiles
{
    public class GenerationProfileValidator : IGenerationProfileValidator
    {
        private IValidationMethods _validationMethods;

        public IValidationMethods ValidationMethods
        {
            get
            {
                Check.RequireInjected(_validationMethods, nameof(ValidationMethods), nameof(GenerationProfileValidator));
                return _validationMethods;
            }
            set { _validationMethods = value; }
        }

        public ValidationResult ValidateName(string profileName)
        {
            ValidationResult maxLengthValidation = ValidationMethods.ValidateStringLength(profileName, 20);

            if(maxLengthValidation.IsValid == false)
            {
                return maxLengthValidation;
            }

            return ValidationMethods.ValidateStringNotNullNotEmpty(profileName);
        }
    }
}
