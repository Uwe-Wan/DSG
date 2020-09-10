using DSG.BusinessEntities;
using DSG.Common;
using DSG.DAO.Expansions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessComponents.Expansions
{
    public class DominionExpansionBc : IDominionExpansionBc
    {
        private IDominionExpansionDao _dominionExpansionDao;

        public IDominionExpansionDao DominionExpansionDao
        {
            get
            {
                Check.RequireInjected(_dominionExpansionDao, nameof(DominionExpansionDao), nameof(DominionExpansionBc));
                return _dominionExpansionDao;
            }
            set { _dominionExpansionDao = value; }
        }

        public List<DominionExpansion> GetExpansions()
        {
            return DominionExpansionDao.GetExpansions();
        }

        public DominionExpansion GetExpansionByName(string expansionName)
        {
            return DominionExpansionDao.GetExpansionByName(expansionName);
        }

        public void InsertExpansion(string expansionName)
        {
            DominionExpansionDao.InsertExpansion(expansionName);
        }
    }
}
