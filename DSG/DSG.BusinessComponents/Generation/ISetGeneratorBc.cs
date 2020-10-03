using DSG.BusinessEntities;
using System.Collections.Generic;

namespace DSG.BusinessComponents.Generation
{
    public interface ISetGeneratorBc
    {
        GeneratedSetDto GenerateSet(List<DominionExpansion> dominionExpansions);
    }
}
