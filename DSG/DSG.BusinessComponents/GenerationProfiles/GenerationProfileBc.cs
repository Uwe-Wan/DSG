using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using DSG.DAO.GenerationProfiles;
using System.Collections.Generic;

namespace DSG.BusinessComponents.GenerationProfiles
{
    public class GenerationProfileBc : IGenerationProfileBc
    {
        private IGenerationProfileDao _generationProfileDao;

        public IGenerationProfileDao GenerationProfileDao
        {
            get
            {
                Check.RequireInjected(_generationProfileDao, nameof(GenerationProfileDao), nameof(GenerationProfileDao));
                return _generationProfileDao;
            }
            set { _generationProfileDao = value; }
        }

        public List<GenerationProfile> GetGenerationProfiles()
        {
            return GenerationProfileDao.GetGenerationProfiles();
        }

        public void InsertGenerationProfile(GenerationProfile generationProfile)
        {
            GenerationProfileDao.InsertGenerationProfile(generationProfile);
        }
    }
}
