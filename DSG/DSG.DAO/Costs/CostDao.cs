using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG.BusinessEntities;

namespace DSG.DAO.Costs
{
    public class CostDao : ICostDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public List<Cost> GetCosts()
        {
            return Ctx.Cost.ToList();
        }
    }
}
