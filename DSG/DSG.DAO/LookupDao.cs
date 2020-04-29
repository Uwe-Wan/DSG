using DSG.BusinessEntities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DSG.DAO
{
    public class LookupDao<TEntity> where TEntity : class
    {
        public List<TEntity> LookupEntities { get; set; }

        public List<TEntity> GetLookup()
        {
            if (LookupEntities != null)
            {
                return LookupEntities;
            }

            CardManagementDbContext ctx = new CardManagementDbContext();

            IQueryable<TEntity> dbQuery = ctx.Set<TEntity>();

            LookupEntities = dbQuery
                .AsNoTracking()
                .ToList();

            return LookupEntities;
        }
    }
}
