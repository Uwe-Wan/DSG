using DSG.BusinessEntities;
using System.Collections.Generic;
using System.Linq;

namespace DSG.DAO.Expansions
{
    public class DominionExpansionDao : IDominionExpansionDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public List<DominionExpansion> GetExpansions()
        {
            return Ctx.DominionExpansion
                .Include("ContainedCards")
                .Include("ContainedCards.Cost")
                .Include("ContainedCards.CardTypeToCards")
                .Include("ContainedCards.CardTypeToCards.CardType")
                .Include("ContainedCards.CardSubTypeToCards")
                .Include("ContainedCards.CardSubTypeToCards.CardSubType")
                .Include("ContainedCards.CardArtifactsToCard")
                .Include("ContainedCards.CardArtifactsToCard.CardArtifact")
                .Include("ContainedArtifacts")
                .ToList();
        }

        public DominionExpansion GetExpansionByName(string expansionName)
        {
            return Ctx.DominionExpansion
                .Include("ContainedCards")
                .Include("ContainedCards.Cost")
                .Include("ContainedCards.CardTypeToCards")
                .Include("ContainedCards.CardTypeToCards.CardType")
                .Include("ContainedCards.CardSubTypeToCards")
                .Include("ContainedCards.CardSubTypeToCards.CardSubType")
                .Include("ContainedCards.CardArtifactsToCard")
                .Include("ContainedCards.CardArtifactsToCard.CardArtifact")
                .Include("ContainedArtifacts")
                .SingleOrDefault(expansion => expansion.Name == expansionName);
        }

        public void InsertExpansion(string expansionName)
        {
            Ctx.DominionExpansion.Add(new DominionExpansion { Name = expansionName });
            Ctx.SaveChanges();
        }
    }
}
