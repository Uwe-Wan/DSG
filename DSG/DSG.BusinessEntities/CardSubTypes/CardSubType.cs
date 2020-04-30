using System.ComponentModel.DataAnnotations;

namespace DSG.BusinessEntities.CardSubTypes
{
    public class CardSubType
    {
        public CardSubTypeEnum Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
