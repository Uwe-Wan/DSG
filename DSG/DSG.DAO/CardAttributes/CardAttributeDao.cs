using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;

namespace DSG.DAO.CardAttributes
{
    public class CardAttributeDao : ICardAttributeDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public void InsertAttribute(CardArtifact attribute)
        {
            Ctx.CardArtifacts.Add(attribute);

            Ctx.SaveChanges();
        }
    }
}
