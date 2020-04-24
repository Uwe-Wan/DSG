using DSG.BusinessEntities;
using System.Collections.Generic;

namespace DSG.BusinessComponents.Generation
{
    public interface ISetGeneratorBc
    {
        List<Card> GenerateSet(List<Card> availableCards);
    }
}
