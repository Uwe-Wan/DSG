using DSG.BusinessEntities.CardTypes;

namespace DSG.BusinessEntities
{
    public class IsCardTypeSelectedDto
    {
        public CardTypeEnum CardType { get; set; }

        public bool IsSelected { get; set; }
    }
}
