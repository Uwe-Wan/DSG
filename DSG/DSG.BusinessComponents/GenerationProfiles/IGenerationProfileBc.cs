using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DSG.BusinessComponents.GenerationProfiles
{
    public interface IGenerationProfileBc
    {
        string InsertGenerationProfile(GenerationProfile generationProfile);

        GenerationProfile PrepareGenerationProfileForInsertion(
            GenerationProfile generationProfile, ObservableCollection<IsSelectedAndWeightedExpansionDto> isDominionExpansionSelectedDtos, IEnumerable<GenerationProfile> existingProfiles);

        List<GenerationProfile> GetGenerationProfiles();

        void DeleteGenerationProfile(GenerationProfile generationProfile);

        GenerationProfile SetupInitialGenerationProfile(GenerationProfile generationProfile);
    }
}
