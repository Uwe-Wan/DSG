using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardTypes
{
    public class CardTypeToCard
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Card")]
        public int CardId { get; set; }

        public Card Card { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("CardType")]
        public CardTypeEnum CardTypeId { get; set; }

        public CardType CardType { get; set; }
    }
}
