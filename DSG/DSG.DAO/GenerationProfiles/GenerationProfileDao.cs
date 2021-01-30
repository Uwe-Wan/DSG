using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DSG.DAO.GenerationProfiles
{
    public class GenerationProfileDao : IGenerationProfileDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public void InsertGenerationProfile(GenerationProfile profile)
        {
            Ctx.GenerationProfiles.Add(profile);

            Ctx.SaveChanges();
        }

        public List<GenerationProfile> GetGenerationProfiles()
        {
            return Ctx.GenerationProfiles
                .Include(profile => profile.PropabilityForNonSupplyCards)
                .Include(profile => profile.SelectedExpansions)
                .ToList();
        }
    }
}
