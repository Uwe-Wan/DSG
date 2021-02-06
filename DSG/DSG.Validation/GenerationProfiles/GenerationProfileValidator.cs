using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using DSG.DAO.GenerationProfiles;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DSG.Validation.GenerationProfiles
{
    public class GenerationProfileValidator : IGenerationProfileValidator
    {
        private IValidationMethods _validationMethods;
        private IGenerationProfileDao _generationProfileDao;

        public IValidationMethods ValidationMethods
        {
            get
            {
                Check.RequireInjected(_validationMethods, nameof(ValidationMethods), nameof(GenerationProfileValidator));
                return _validationMethods;
            }
            set { _validationMethods = value; }
        }

        public IGenerationProfileDao GenerationProfileDao
        {
            get
            {
                Check.RequireInjected(_generationProfileDao, nameof(GenerationProfileDao), nameof(GenerationProfileValidator));
                return _generationProfileDao;
            }
            set { _generationProfileDao = value; }
        }

        public ValidationResult ValidateName(string profileName)
        {
            ValidationResult maxLengthValidation = ValidationMethods.ValidateStringLength(profileName, 20);

            if (maxLengthValidation.IsValid == false)
            {
                return maxLengthValidation;
            }

            return ValidationMethods.ValidateStringNotNullNotEmpty(profileName);
        }

        public ValidationResult ValidateNameNoDuplicate(string profileName, IEnumerable<string> existingProfileNames)
        {
            return ValidationMethods.ValidateNameNoDuplicate(profileName, existingProfileNames, nameof(GenerationProfile));
        }

        public ValidationResult ValidateNameNoDuplicate(string profileName)
        {
            IEnumerable<string> existingProfileNames = GenerationProfileDao.GetGenerationProfiles().Select(profile => profile.Name);

            return ValidationMethods.ValidateNameNoDuplicate(profileName, existingProfileNames, nameof(GenerationProfile));
        }
    }
}
