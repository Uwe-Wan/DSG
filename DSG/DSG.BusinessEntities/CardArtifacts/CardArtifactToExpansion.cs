using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class CardArtifactToExpansion
    {
        [Key, Column(Order = 1)]
        [ForeignKey("DominionExpansion")]
        public int DominionExpansionId { get; set; }

        public DominionExpansion DominionExpansion { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("CardArtifact")]
        public int CardArtifactId { get; set; }

        public CardArtifact CardArtifact { get; set; }
    }
}
