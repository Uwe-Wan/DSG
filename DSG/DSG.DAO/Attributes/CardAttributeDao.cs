using DSG.BusinessEntities;
using DSG.BusinessEntities.CardAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.DAO.Attributes
{
    public class CardAttributeDao : ICardAttributeDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public void InsertAttribute(CardAttribute attribute)
        {
            Ctx.CardAttribute.Add(attribute);

            Ctx.SaveChanges();
        }
    }
}
