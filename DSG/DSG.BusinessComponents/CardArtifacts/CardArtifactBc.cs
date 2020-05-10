using DSG.BusinessEntities.CardArtifacts;
using DSG.DAO.CardArtifacts;

namespace DSG.BusinessComponents.CardArtifacts
{
    public class CardArtifactBc : ICardArtifactBc
    {
        private ICardArtifactDao _cardArtifactDao;

        public ICardArtifactDao CardArtifactDao
        {
            get { return _cardArtifactDao; }
            set { _cardArtifactDao = value; }
        }

        public void InsertArtifact(CardArtifact artifact)
        {
            CardArtifactDao.InsertArtifact(artifact);
        }
    }
}
