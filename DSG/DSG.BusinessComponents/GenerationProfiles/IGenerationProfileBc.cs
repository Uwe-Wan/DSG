using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;

namespace DSG.BusinessComponents.GenerationProfiles
{
    public interface IGenerationProfileBc
    {
        void InsertGenerationProfile(GenerationProfile generationProfile);

        List<GenerationProfile> GetGenerationProfiles();
    }
}
