using DSG.BusinessEntities.CardAttributes;
using DSG.DAO.CardAttributes;
using DSG.DAO.Cards;

namespace DSG.BusinessComponents.CardAttributes
{
    public class CardAttributeBc : ICardAttributeBc
    {
        private ICardAttributeDao _cardAttributeDao;

        public ICardAttributeDao CardAttributeDao
        {
            get { return _cardAttributeDao; }
            set { _cardAttributeDao = value; }
        }

        public void InsertAttribute(CardAttribute attribute)
        {
            CardAttributeDao.InsertAttribute(attribute);
        }
    }
}
