using DSG.BusinessEntities;
using System.Collections.Generic;

namespace DSG.DAO.Expansions
{
    public interface IDominionExpansionDao
    {
        List<DominionExpansion> GetExpansions();

        void InsertExpansion(string expansionName);

        DominionExpansion GetExpansionByName(string expansionName);
    }
}
