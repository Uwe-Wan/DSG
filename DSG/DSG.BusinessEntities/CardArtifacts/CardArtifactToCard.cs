using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class CardArtifactToCard
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Card")]
        public int CardId { get; set; }

        public Card Card { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("CardArtifact")]
        public int CardArtifactId { get; set; }

        public CardArtifact CardArtifact { get; set; }
    }
}
