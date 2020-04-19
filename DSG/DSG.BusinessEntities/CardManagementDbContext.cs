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

    }
}
