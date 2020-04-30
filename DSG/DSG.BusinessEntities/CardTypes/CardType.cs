using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardTypes
{
    [Table("CardType")]
    public class CardType
    {
        public CardTypeEnum Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsSupplyType { get; set; }
    }
}
