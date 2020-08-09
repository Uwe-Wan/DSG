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
                .Include("ContainedCards.CardArtifacts")
                .ToList();

            //DbQuery<DominionExpansion> query = Ctx.DominionExpansion
            //    .Include("ContainedCards")
            //    .Include("ContainedCards.Cost");
            //    //.Include("ContainedCards.CardTypeToCards")
            //    //.Include("ContainedCards.CardTypeToCards.CardType")
            //    //.Include("ContainedCards.CardSubTypeToCards")
            //    //.Include("ContainedCards.CardSubTypeToCards.CardSubType");
            //    //.Include("ContainedCards.CardArtifacts");
            //    List<DominionExpansion> expansions = query.ToList();
            //return expansions;
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
                .Include("ContainedArtifactsToExpansion")
                .Include("ContainedArtifactsToExpansion.CardArtifact")
                .SingleOrDefault(expansion => expansion.Name == expansionName);
        }

        public void InsertExpansion(string expansionName)
        {
            Ctx.DominionExpansion.Add(new DominionExpansion { Name = expansionName });
            Ctx.SaveChanges();
        }
    }
}
