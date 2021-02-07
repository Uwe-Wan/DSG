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

        public void DeleteGenerationProfile(GenerationProfile generationProfile)
        {
            Ctx.GenerationProfiles.Remove(generationProfile);

            Ctx.SaveChanges();
        }

        public void DeletePropabilityForNonSupplyCardsById(int id)
        {
            PropabilityForNonSupplyCards propability = Ctx.PropabilityForNonSupplyCards.Single(x => x.Id == id);
            Ctx.PropabilityForNonSupplyCards.Remove(propability);

            Ctx.SaveChanges();
        }

        public bool IsPropabilitiesForNonSupplyCardsStillUsed(int propabilitiesId)
        {
            return Ctx.GenerationProfiles
                .Where(profile => profile.PropabilityForNonSupplyCardsId == propabilitiesId)
                .Any();
        }

        public void DeleteSelectedExpansionToGenerationProfilesByProfileId(int generationProfileId)
        {
            List<SelectedExpansionToGenerationProfile> selectedExpansionsOfProfile = Ctx.SelectedExpansionsToGenerationProfiles
                .Where(x => x.GenerationProfileId == generationProfileId)
                .ToList();
            Ctx.SelectedExpansionsToGenerationProfiles.RemoveRange(selectedExpansionsOfProfile);

            Ctx.SaveChanges();
        }
    }
}
