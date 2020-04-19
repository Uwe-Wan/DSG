using System.Collections.Generic;
using DSG.BusinessEntities;
using DSG.DAO.Costs;

namespace DSG.BusinessComponents.Costs
{
    public class CostBc : ICostBc
    {
        private ICostDao _costDao;

        public ICostDao CostDao
        {
            get { return _costDao; }
            set { _costDao = value; }
        }
        public List<Cost> GetCosts()
        {
            return CostDao.GetCosts();
        }
    }
}
