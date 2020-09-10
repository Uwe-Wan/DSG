using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.DAO.CardArtifacts;

namespace DSG.BusinessComponents.CardArtifacts
{
    public class CardArtifactBc : ICardArtifactBc
    {
        private ICardArtifactDao _cardArtifactDao;

        public ICardArtifactDao CardArtifactDao
        {
            get
            {
                Check.RequireInjected(CardArtifactDao, nameof(CardArtifactDao), nameof(CardArtifactBc));
                return _cardArtifactDao;
            }
            set { _cardArtifactDao = value; }
        }

        public void InsertArtifact(CardArtifact artifact)
        {
            CardArtifactDao.InsertArtifact(artifact);
        }
    }
}
