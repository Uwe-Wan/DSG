using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Cost = card.Cost.Potion ? card.Cost.Money + card.Cost.Dept + "+T" : (card.Cost.Money + card.Cost.Dept).ToString();
            Set = card.DominionExpansion.Name;
            Type = string.Join(", ", card.CardTypeToCards.Select(x => x.CardType.Name));
        }

        public CardAndArtifactViewEntity(GeneratedAdditionalCard generatedAdditional)
        {
            Name = generatedAdditional.AdditionalCard.Name;
            BelongsTo = generatedAdditional.Parent.Name;
            int moneyPlusDept = generatedAdditional.AdditionalCard.Cost.Money + generatedAdditional.AdditionalCard.Cost.Dept;
            Cost = generatedAdditional.AdditionalCard.Cost.Potion ? moneyPlusDept + "+T" : moneyPlusDept.ToString();
            Set = generatedAdditional.AdditionalCard.DominionExpansion.Name;
            Type = string.Join(", ", generatedAdditional.AdditionalCard.CardTypeToCards.Select(x => x.CardType.Name));
        }

        public CardAndArtifactViewEntity(CardArtifact artifact)
        {
            Name = artifact.Name;
            Set = artifact.DominionExpansion.Name;
            Type = "Artifact";
        }
    }
}
