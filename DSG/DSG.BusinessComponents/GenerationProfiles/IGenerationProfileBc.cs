using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;

namespace DSG.BusinessComponents.GenerationProfiles
{
    public interface IGenerationProfileBc
    {
        string InsertGenerationProfile(GenerationProfile generationProfile);

        List<GenerationProfile> GetGenerationProfiles();

        void DeleteGenerationProfile(GenerationProfile generationProfile);

        GenerationProfile SetupInitialGenerationProfile(GenerationProfile generationProfile);
    }
}
