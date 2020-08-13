using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardArtifacts
{
    [Table("CardArtifact")]
    public class CardArtifact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("UQX_CardArtifact_Name_DominionExpansion", 1, IsUnique = true)]
        public string Name { get; set; }

        [ForeignKey("AdditionalCard")]
        public int? AdditionalCardId { get; set; }
        public AdditionalCard AdditionalCard { get; set; }

        public int AmountOfAdditionalCards { get; set; }

        public DominionExpansion DominionExpansion { get; set; }

        [Required]
        [ForeignKey("DominionExpansion")]
        [Index("UQX_CardArtifact_Name_DominionExpansion", 2, IsUnique = true)]
        public int DominionExpansionId { get; set; }
    }
}
