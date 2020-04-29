using DSG.DAO;
using System.Collections.Generic;

namespace DSG.BusinessComponents
{
    public class LookupBc<TEntity> where TEntity : class
    {
        private LookupDao<TEntity> _lookupDao;

        public LookupDao<TEntity> LookupDao
        {
            get { return _lookupDao ?? new LookupDao<TEntity>(); }
            set { _lookupDao = value; }
        }

        public List<TEntity> GetLookup()
        {
            return LookupDao.GetLookup();
        }
    }
}
