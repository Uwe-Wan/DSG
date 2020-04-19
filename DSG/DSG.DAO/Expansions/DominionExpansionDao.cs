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
                .ToList();
        }

        public DominionExpansion GetExpansionByName(string expansionName)
        {
            return Ctx.DominionExpansion
                .Include("ContainedCards")
                .SingleOrDefault(expansion => expansion.Name == expansionName);

            //string sqlCmd = "SELECT * FROM DominionExpansion WHERE Name = {0}";

            //DataLoadOptions dlo = new DataLoadOptions();
            //dlo.LoadWith<DominionExpansion>(expansion => expansion.ContainedCards);
            //DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            //ctx.LoadOptions = dlo;
            //return ctx.ExecuteQuery<DominionExpansion>(sqlCmd, expansionName).SingleOrDefault();
        }

        public void InsertExpansion(string expansionName)
        {
            Ctx.DominionExpansion.Add(new DominionExpansion { Name = expansionName });
            Ctx.SaveChanges();
        }
    }
}
