using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG.BusinessEntities;

namespace DSG.DAO.Costs
{
    public interface ICostDao
    {
        List<Cost> GetCosts();
    }
}
