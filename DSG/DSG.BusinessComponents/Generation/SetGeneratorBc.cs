using DSG.BusinessComponents.Probabilities;
using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using static DSG.BusinessComponents.StaticMethods.CardHelper;

namespace DSG.BusinessComponents.Generation
{
    public class SetGeneratorBc : ISetGeneratorBc
    {
        private IGetIntByProbabilityBc _getIntByProbabilityBc;
        private IShuffleListBc<Card> _shuffleListBc;

        public IGetIntByProbabilityBc GetIntByProbabilityBc
        {
            get
            {
                Check.RequireInjected(_getIntByProbabilityBc, nameof(GetIntByProbabilityBc), nameof(SetGeneratorBc));
                return _getIntByProbabilityBc;
            }
            set { _getIntByProbabilityBc = value; }
        }

        public IShuffleListBc<Card> ShuffleListBc
        {
            get
            {
                Check.RequireInjected(_shuffleListBc, nameof(ShuffleListBc), nameof(SetGeneratorBc));
                return _shuffleListBc;
            }
            set { _shuffleListBc = value; }
        }

        public GeneratedSetDto GenerateSet(List<DominionExpansion> dominionExpansions)
        {
            List<Card> availableCards = dominionExpansions.SelectMany(expansion => expansion.ContainedCards).ToList();

            List<Card> chosenSupplyCards = ChooseSupplyCards(availableCards);
            List<Card> chosenNonSupplyCards = ChooseNonSupplyCards(availableCards);
            List<Card> temporarlySet = chosenSupplyCards.Union(chosenNonSupplyCards).ToList();

            List<Card> availableSupplyTypeCards = availableCards
                .Where(card => temporarlySet.Contains(card) == false)
                .GetSupplyCards();

            List<GeneratedAdditionalCard> generatedAdditionalCards = GetAdditionalCards(availableSupplyTypeCards, temporarlySet);
            List<GeneratedAdditionalCard> generatedExistingAdditionalCards = GetExistingAdditionalCards(chosenSupplyCards, temporarlySet);

            GeneratedSetDto generatedSetDto = new GeneratedSetDto(chosenSupplyCards, chosenNonSupplyCards, generatedAdditionalCards, generatedExistingAdditionalCards);

            return generatedSetDto;
        }

        private List<GeneratedAdditionalCard> GetExistingAdditionalCards(List<Card> supplyCards, List<Card> temporarlySet)
        {
            return ChooseAdditionalCardsIsAlreadyIncluded(true, supplyCards, temporarlySet);
        }

        private List<GeneratedAdditionalCard> GetAdditionalCards(List<Card> availableCards, IEnumerable<Card> temporarlySet)
        {
            List<GeneratedAdditionalCard> additionalCardsForSet = ChooseAdditionalCardsIsAlreadyIncluded(false, availableCards, temporarlySet);

            if (additionalCardsForSet.Count == 0)
            {
                return new List<GeneratedAdditionalCard>();
            }

            additionalCardsForSet.AddRange(
                GetAdditionalCards(availableCards, additionalCardsForSet.Select(x => x.AdditionalCard)));

            return additionalCardsForSet;
        }

        private List<GeneratedAdditionalCard> ChooseAdditionalCardsIsAlreadyIncluded(bool alreadyIncluded, List<Card> cardsToChooseFrom, IEnumerable<Card> temporarlySet)
        {
            List<Card> cardsWithAdditionalCards = temporarlySet
                .Where(card => card.GetAdditionalCardsAlreadyIncluded(alreadyIncluded)?.Any() == true)
                .ToList();

            List<GeneratedAdditionalCard> additionalCardsForSet = new List<GeneratedAdditionalCard>();

            foreach (Card card in cardsWithAdditionalCards)
            {
                foreach (CardArtifact artifact in card.GetArtifactsWithAdditional(alreadyIncluded))
                {
                    // parent needs to be temporarly removed since we do not want to choose one card as its own additional
                    if (alreadyIncluded)
                    {
                        cardsToChooseFrom.Remove(card);
                    }

                    ChooseAdditionalCardForParentCard(artifact, card, additionalCardsForSet, cardsToChooseFrom);

                    if (alreadyIncluded && card.IsSupplyType())
                    {
                        cardsToChooseFrom.Add(card);
                    }
                }
            }

            return additionalCardsForSet;
        }

        private void ChooseAdditionalCardForParentCard(CardArtifact artifact, Card parent, 
            List<GeneratedAdditionalCard> additionalCardsForSet, List<Card> cardsToChooseFrom)
        {
            Check.RequireNotNull(artifact.AmountOfAdditionalCards, nameof(artifact.AmountOfAdditionalCards), nameof(SetGeneratorBc));

            AdditionalCard additionalCard = artifact.AdditionalCard;

            List<Card> cardsWithAllowedCost = cardsToChooseFrom
                .Where(x => additionalCard.MinCost.HasValue == false || x.Cost.Money >= additionalCard.MinCost.Value)
                .Where(x => additionalCard.MaxCost.HasValue == false || x.Cost.Money <= additionalCard.MaxCost.Value)
                .ToList();
            List<Card> generatedAdditionalCards = ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(cardsWithAllowedCost, artifact.AmountOfAdditionalCards.Value);
            additionalCardsForSet.AddRange(generatedAdditionalCards.Select(card => new GeneratedAdditionalCard(card, parent)));
            //the way this is currently written, it is also ensured that an existing card is not used as additional card for two cards
            cardsToChooseFrom.RemoveAll(x => x.Equals(generatedAdditionalCards));
        }

        private List<Card> ChooseSupplyCards(List<Card> availableCards)
        {
            List<Card> availableSupplyCards = availableCards.GetSupplyCards();

            if (availableSupplyCards.Count < 10)
            {
                throw new NotEnoughCardsAvailableException("Not enough Cards available.");
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableSupplyCards, 10);
        }

        private List<Card> ChooseNonSupplyCards(List<Card> availableCards)
        {
            List<Card> availableNonSupplyCards = availableCards.GetNonSupplyCards();

            int numberOfNonSupplyCards = GetIntByProbabilityBc.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7);

            if (numberOfNonSupplyCards > availableNonSupplyCards.Count)
            {
                return availableNonSupplyCards;
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableNonSupplyCards, numberOfNonSupplyCards);
        }
    }
}
