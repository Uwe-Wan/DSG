using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessEntities
{
    public class CardManagementDbContext : DbContext
    {
        public CardManagementDbContext() : base("DSGConnectionString") { }

        public virtual DbSet<Cost> Cost { get; set; }

        public virtual DbSet<Card> Card { get; set; }

        public virtual DbSet<DominionExpansion> DominionExpansion { get; set; }

        public virtual DbSet<CardType> CardType { get; set; }

        public virtual DbSet<CardTypeToCard> CardTypeToCard { get; set; }

        public virtual DbSet<CardSubType> CardSubType { get; set; }

        public virtual DbSet<CardSubTypeToCard> CardSubTypeToCard { get; set; }

    }
}
