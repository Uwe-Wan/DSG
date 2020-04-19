using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.DAO.Test
{
    public class AbstractDaoTestSetup
    {
        public void CleanDatabase()
        {
            CardManagementDbContext ctx = new CardManagementDbContext();
            ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE DominionExpansions");
            ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE Cards");
            ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE Costs");
        }

        public void SetupBasicExpansions()
        {
            CardManagementDbContext ctx = new CardManagementDbContext();
            DominionExpansion world = new DominionExpansion { Name = "World" };
            DominionExpansion seaside = new DominionExpansion { Name = "Seaside" };
            DominionExpansion intrigue = new DominionExpansion { Name = "Intrigue" };
            ctx.DominionExpansion.AddRange(new List<DominionExpansion> { world, seaside, intrigue });
            ctx.SaveChanges();
        }

        public void SetUpCards()
        {
            CardManagementDbContext ctx = new CardManagementDbContext();

            List<DominionExpansion> dominionExpansions = ctx.DominionExpansion.ToList();

            int worldId = dominionExpansions.Single(expansion => expansion.Name == "World").Id;
            int seasideId = dominionExpansions.Single(expansion => expansion.Name == "Seaside").Id;
            int intrigueId = dominionExpansions.Single(expansion => expansion.Name == "Intrigue").Id;

            Cost two = new Cost { Money = 2 };

            Card chapel = new Card { DominionExpansionId = worldId, Name = "Chapel", CostId = two.Id };
        }
    }
}
