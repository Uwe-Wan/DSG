using DSG.BusinessEntities.CardAttributes;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using System.Data.Entity;

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

        public virtual DbSet<CardAttribute> CardAttribute {get; set;}

        public virtual DbSet<AdditionalCard> AdditionalCard { get; set; }
    }
}
