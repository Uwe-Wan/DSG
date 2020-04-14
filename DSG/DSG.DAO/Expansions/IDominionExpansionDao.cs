using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.DAO.Expansions
{
    public interface IDominionExpansionDao
    {
        List<DominionExpansion> GetExpansions();

        void InsertExpansion(string expansionName);

        DominionExpansion GetExpansionByName(string expansionName);
    }
}
