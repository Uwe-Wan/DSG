using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using System.Linq;

namespace DSG.Presentation.ViewEntity
{
    public class CardAndArtifactViewEntity
    {
        public string Name { get; set; }

        public string BelongsTo { get; set; }

        public string Cost { get; set; }

        public string Set { get; set; }

        public string Type { get; set; }

        public CardAndArtifactViewEntity(Card card)
        {
            Name = card.Name;
            Cost = FormatCost(card);
            Set = card.DominionExpansion.Name;
            Type = string.Join(", ", card.CardTypeToCards.Select(x => x.CardType.Name));
        }

        public CardAndArtifactViewEntity(GeneratedAdditionalCard generatedAdditional)
        {
            Name = generatedAdditional.AdditionalCard.Name;
            BelongsTo = generatedAdditional.Parent.Name;
            Cost = FormatCost(generatedAdditional.AdditionalCard);
            Set = generatedAdditional.AdditionalCard.DominionExpansion.Name;
            Type = string.Join(", ", generatedAdditional.AdditionalCard.CardTypeToCards.Select(x => x.CardType.Name));
        }

        public CardAndArtifactViewEntity(CardArtifact artifact)
        {
            Name = artifact.Name;
            Set = artifact.DominionExpansion.Name;
            Type = "Artifact";
        }

        private string FormatCost(Card card)
        {
            int? moneyPlusDept = 0;

            if(card.Cost?.Money == null && card.Cost.Dept == null)
            {
                moneyPlusDept = null;
            }
            else
            {
                moneyPlusDept += card.Cost?.Money ?? 0;
                moneyPlusDept += card.Cost?.Dept ?? 0;
            }

            if (card.Cost.Potion)
            {
                return moneyPlusDept + "+T";
            }

            return moneyPlusDept.ToString();
        }
    }
}
