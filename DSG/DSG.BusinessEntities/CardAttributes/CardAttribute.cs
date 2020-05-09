using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardAttributes
{
    public class CardAttribute
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("UQX_CardAttribute_Name_DominionExpansionId", 1, IsUnique = true)]
        public string Name { get; set; }

        [ForeignKey("DominionExpansion")]
        [Index("UQX_CardAttribute_Name_DominionExpansionId", 2, IsUnique = true)]
        public int? DominionExpansionId { get; set; }
        public DominionExpansion DominionExpansion { get; set; }

        public List<Card> Cards { get; set; }

        [ForeignKey("AdditionalCard")]
        public int? AdditionalCardId { get; set; }
        public AdditionalCard AdditionalCard { get; set; }
    }
}
