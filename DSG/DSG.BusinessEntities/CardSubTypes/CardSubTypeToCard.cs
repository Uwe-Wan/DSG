using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardSubTypes
{
    public class CardSubTypeToCard
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Card")]
        public int CardId { get; set; }

        public Card Card { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("CardSubType")]
        public CardSubTypeEnum CardSubTypeId { get; set; }

        public CardSubType CardSubType { get; set; }
    }
}
