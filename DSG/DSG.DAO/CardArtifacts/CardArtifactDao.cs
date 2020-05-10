using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;

namespace DSG.DAO.CardArtifacts
{
    public class CardArtifactDao : ICardArtifactDao
    {
        private CardManagementDbContext _ctx;

        public CardManagementDbContext Ctx
        {
            get { return _ctx; }
            set { _ctx = value; }
        }

        public void InsertArtifact(CardArtifact artifact)
        {
            Ctx.CardArtifacts.Add(artifact);

            Ctx.SaveChanges();
        }
    }
}
