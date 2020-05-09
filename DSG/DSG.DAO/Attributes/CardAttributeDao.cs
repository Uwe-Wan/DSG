using DSG.BusinessEntities;
using DSG.BusinessEntities.CardAttributes;

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
