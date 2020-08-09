using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class CardArtifact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("UQX_CardArtifact_Name", 1, IsUnique = true)]
        public string Name { get; set; }

        public List<Card> Cards { get; set; }

        [ForeignKey("AdditionalCard")]
        public int? AdditionalCardId { get; set; }
        public AdditionalCard AdditionalCard { get; set; }

        public int AmountOfAdditionalCards { get; set; }
    }
}
