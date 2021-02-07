using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using DSG.DAO.GenerationProfiles;
using DSG.Validation.GenerationProfiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace DSG.BusinessComponents.GenerationProfiles
{
    public class GenerationProfileBc : IGenerationProfileBc
    {
        private IGenerationProfileDao _generationProfileDao;
        private IGenerationProfileValidator _generationProfileValidator;

        public IGenerationProfileDao GenerationProfileDao
        {
            get
            {
                Check.RequireInjected(_generationProfileDao, nameof(GenerationProfileDao), nameof(GenerationProfileBc));
                return _generationProfileDao;
            }
            set { _generationProfileDao = value; }
        }

        public IGenerationProfileValidator GenerationProfileValidator
        {
            get
            {
                Check.RequireInjected(_generationProfileDao, nameof(GenerationProfileValidator), nameof(GenerationProfileBc));
                return _generationProfileValidator;
            }
            set { _generationProfileValidator = value; }
        }

        public List<GenerationProfile> GetGenerationProfiles()
        {
            return GenerationProfileDao.GetGenerationProfiles();
        }

        public string InsertGenerationProfile(GenerationProfile generationProfile)
        {
            string validationError = ValidateProfileName(generationProfile);
            if (validationError != null)
            {
                return validationError;
            }

            GenerationProfileDao.InsertGenerationProfile(generationProfile);

            return null;
        }

        public GenerationProfile PrepareGenerationProfileForInsertion(
            GenerationProfile generationProfile, ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos, IEnumerable<GenerationProfile> existingProfiles)
        {
            GenerationProfile newProfile = generationProfile.Clone();

            newProfile.SelectedExpansions = isDominionExpansionSelectedDtos
                .Where(x => x.IsSelected)
                .Select(x => new SelectedExpansionToGenerationProfile(x.DominionExpansion))
                .ToList();

            PropabilityForNonSupplyCards existingPropability = existingProfiles
                .Select(x => x.PropabilityForNonSupplyCards)
                .SingleOrDefault(x => x.Equals(newProfile.PropabilityForNonSupplyCards));
            if (existingPropability != null)
            {
                newProfile.PropabilityForNonSupplyCards = existingPropability;
            }

            return newProfile;
        }

        private string ValidateProfileName(GenerationProfile generationProfile)
        {
            ValidationResult isNameValid = GenerationProfileValidator.ValidateName(generationProfile.Name);
            if (isNameValid.IsValid == false)
            {
                return isNameValid.ErrorContent.ToString() + " " + Text.ProfileNotSaved;
            }

            ValidationResult nameIsNoDuplicate = GenerationProfileValidator.ValidateNameNoDuplicate(generationProfile.Name);

            if (nameIsNoDuplicate.IsValid == false)
            {
                return nameIsNoDuplicate.ErrorContent.ToString() + " " + Text.ProfileNotSaved;
            }

            return null;
        }

        public void DeleteGenerationProfile(GenerationProfile generationProfile)
        {
            GenerationProfileDao.DeleteGenerationProfile(generationProfile);
        }

        public GenerationProfile SetupInitialGenerationProfile(GenerationProfile generationProfile)
        {
            if (generationProfile == null)
            {
                generationProfile = new GenerationProfile(
                    10,
                    20,
                    SetupInitialPropabilitiesForNonSupplies());
            }

            return generationProfile;
        }

        private PropabilityForNonSupplyCards SetupInitialPropabilitiesForNonSupplies()
        {
            PropabilityForNonSupplyCards propabilityForNonSupplyCards = new PropabilityForNonSupplyCards(50, 30, 7, 0);

            return propabilityForNonSupplyCards;
        }
    }
}
