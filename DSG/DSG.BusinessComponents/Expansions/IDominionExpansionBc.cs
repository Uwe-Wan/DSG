using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessComponents.Expansions
{
    public interface IDominionExpansionBc
    {
        List<DominionExpansion> GetExpansions();

        DominionExpansion GetExpansionByName(string expansionName);

        void InsertExpansion(string expansionName);
    }
}
