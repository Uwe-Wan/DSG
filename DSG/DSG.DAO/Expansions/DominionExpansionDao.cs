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
        public List<DominionExpansion> GetExpansions()
        {
            string sqlCmd = "SELECT * FROM DominionExpansion";

            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<DominionExpansion>(expansion => expansion.ContainedCards);
            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            ctx.LoadOptions = dlo;
            return ctx.ExecuteQuery<DominionExpansion>(sqlCmd).ToList();
        }

        public DominionExpansion GetExpansionByName(string expansionName)
        {
            string sqlCmd = "SELECT * FROM DominionExpansion WHERE Name = {0}";

            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<DominionExpansion>(expansion => expansion.ContainedCards);
            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            ctx.LoadOptions = dlo;
            return ctx.ExecuteQuery<DominionExpansion>(sqlCmd, expansionName).SingleOrDefault();
        }

        public void InsertExpansion(string expansionName)
        {
            string sqlCmd = "INSERT INTO dbo.DominionExpansion (Name) VALUES ({0})";

            DataContext ctx = new DataContext(ConfigurationManager.AppSettings["DSGConnectionString"]);
            ctx.ExecuteCommand(sqlCmd, expansionName);
        }
    }
}
