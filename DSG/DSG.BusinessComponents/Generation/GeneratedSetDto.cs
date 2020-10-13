using DSG.BusinessEntities.CardArtifacts;
using System.Collections.Generic;
using System.Linq;
using static DSG.BusinessComponents.StaticMethods.CardHelper;

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

            FillArtifacts();
        }

        private void FillArtifacts()
        {
            ArtifactsWithoutAdditionalCards = new List<CardArtifact>();

            ArtifactsWithoutAdditionalCards.AddRange(
                SupplyCardsWithoutAdditional.GetArtifactsWithoutAdditional());

            ArtifactsWithoutAdditionalCards.AddRange(
                NonSupplyCards.GetArtifactsWithoutAdditional());

            ArtifactsWithoutAdditionalCards.AddRange(
                GeneratedAdditionalCards
                .Select(x => x.AdditionalCard)
                .GetArtifactsWithoutAdditional());

            ArtifactsWithoutAdditionalCards.AddRange(
                GeneratedExistingAdditionalCards
                .Select(x => x.AdditionalCard)
                .GetArtifactsWithoutAdditional());

            ArtifactsWithoutAdditionalCards = ArtifactsWithoutAdditionalCards.Distinct().ToList();
        }
    }
}
