using DSG.DAO.Cards;
using DSG.BusinessEntities;

namespace DSG.BusinessComponents.Cards
{
    public class CardBc : ICardBc
    {
        private ICardDao _cardDao;

        public ICardDao CardDao
        {
            get { return _cardDao; }
            set { _cardDao = value; }
        }

        public void InsertCard(Card card)
        {
            CardDao.InsertCard(card);
        }
    }
}
