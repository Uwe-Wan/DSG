using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;

namespace DSG.DAO.GenerationProfiles
{
    public interface IGenerationProfileDao
    {
        void InsertGenerationProfile(GenerationProfile generationProfile);

        List<GenerationProfile> GetGenerationProfiles();

        void DeleteGenerationProfile(GenerationProfile generationProfile);
    }
}
