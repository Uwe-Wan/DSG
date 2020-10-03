using DSG.BusinessEntities.CardArtifacts;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessEntities
{
    public class GeneratedSetDto
    {
        public List<GeneratedAdditionalCard> GeneratedAdditionalCards { get; set; }

        public List<GeneratedAdditionalCard> GeneratedExistingAdditionalCards { get; set; }

        public List<Card> SupplyCardsWithoutAdditional { get; set; }

        public List<Card> NonSupplyCards { get; set; }

        public List<CardArtifact> ArtifactsWithoutAdditionalCards { get; set; }

        public GeneratedSetDto(List<Card> supplyCardsWithoutAdditional, List<Card> nonSupplyCards, 
            List<GeneratedAdditionalCard> generatedAdditionalCards, List<GeneratedAdditionalCard> generatedExistingAdditionalCards)
        {
            SupplyCardsWithoutAdditional = supplyCardsWithoutAdditional;
            NonSupplyCards = nonSupplyCards;
            GeneratedAdditionalCards = generatedAdditionalCards;
            GeneratedExistingAdditionalCards = generatedExistingAdditionalCards;

            ArtifactsWithoutAdditionalCards = new List<CardArtifact>();
            ArtifactsWithoutAdditionalCards.AddRange(
                SupplyCardsWithoutAdditional
                .SelectMany(card => card.CardArtifactsToCard)
                .Select(x => x.CardArtifact)
                .Where(x => x.AdditionalCard == null));
            ArtifactsWithoutAdditionalCards.AddRange(
                NonSupplyCards
                .SelectMany(card => card.CardArtifactsToCard)
                .Select(x => x.CardArtifact)
                .Where(x => x.AdditionalCard == null));
            ArtifactsWithoutAdditionalCards.AddRange(
                GeneratedAdditionalCards
                .Select(x => x.AdditionalCard)
                .SelectMany(card => card.CardArtifactsToCard)
                .Select(x => x.CardArtifact)
                .Where(x => x.AdditionalCard == null));
            ArtifactsWithoutAdditionalCards.AddRange(
                GeneratedExistingAdditionalCards
                .Select(x => x.AdditionalCard)
                .SelectMany(card => card.CardArtifactsToCard)
                .Select(x => x.CardArtifact)
                .Where(x => x.AdditionalCard == null));
        }
    }
}
