using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .ToList();
        }

        public DominionExpansion GetExpansionByName(string expansionName)
        {
            return Ctx.DominionExpansion
                .Include("ContainedCards")
                .Include("ContainedCards.Cost")
                //.Include("ContainedCards.CardTypeToCard")
                //.Include("ContainedCards.CardTypeToCard.CardType")
                .SingleOrDefault(expansion => expansion.Name == expansionName);
        }

        public void InsertExpansion(string expansionName)
        {
            Ctx.DominionExpansion.Add(new DominionExpansion { Name = expansionName });
            Ctx.SaveChanges();
        }
    }
}
