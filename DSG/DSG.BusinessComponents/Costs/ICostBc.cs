using System.Collections.Generic;
using DSG.BusinessEntities;

namespace DSG.BusinessComponents.Costs
{
    public interface ICostBc
    {
        List<Cost> GetCosts();

        void InsertCost();
    }
}
