using DSG.BusinessComponents.Probabilities;
using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

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

            List<Card> availableSupplyTypeCards = CardHelper.GetSupplyCards(availableCards.Where(card => temporarlySet.Contains(card) == false).ToList());

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
                .Where(card => CardHelper.GetAdditionalCardsAlreadyIncluded(card, alreadyIncluded).Any())
                .ToList();

            List<GeneratedAdditionalCard> additionalCardsForSet = new List<GeneratedAdditionalCard>();

            foreach (Card card in cardsWithAdditionalCards)
            {
                foreach (AdditionalCard additionalCard in CardHelper.GetAdditionalCardsAlreadyIncluded(card, alreadyIncluded))
                {
                    if (alreadyIncluded)
                    {
                        cardsToChooseFrom.Remove(card);
                    }

                    ChooseAdditionalCardForParentCard(additionalCard, card, additionalCardsForSet, cardsToChooseFrom);

                    if (alreadyIncluded)
                    {
                        cardsToChooseFrom.Add(card);
                    }
                }
            }

            return additionalCardsForSet;
        }

        private void ChooseAdditionalCardForParentCard(AdditionalCard additionalCard, Card parent, 
            List<GeneratedAdditionalCard> additionalCardsForSet, List<Card> cardsToChooseFrom)
        {
            List<Card> cardsWithAllowedCost = cardsToChooseFrom
                .Where(x => additionalCard.MinCost.HasValue == false || x.Cost.Money >= additionalCard.MinCost.Value)
                .Where(x => additionalCard.MaxCost.HasValue == false || x.Cost.Money <= additionalCard.MaxCost.Value)
                .ToList();
            // parent needs to be temporarly removed since we do not want to choose one card as its own additional
            Card generatedAdditionalCard = ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(cardsWithAllowedCost, 1).Single();
            additionalCardsForSet.Add(new GeneratedAdditionalCard(generatedAdditionalCard, parent));
            //the way this is currently written, it is also ensured that an existing card is not used as additional card for two cards
            cardsToChooseFrom.Remove(generatedAdditionalCard);
        }

        private List<Card> ChooseSupplyCards(List<Card> availableCards)
        {
            List<Card> availableSupplyCards = CardHelper.GetSupplyCards(availableCards);

            if (availableSupplyCards.Count < 10)
            {
                throw new NotEnoughCardsAvailableException("Not enough Cards available.");
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableSupplyCards, 10);
        }

        private List<Card> ChooseNonSupplyCards(List<Card> availableCards)
        {
            List<Card> availableNonSupplyCards = CardHelper.GetNonSupplyCards(availableCards);

            int numberOfNonSupplyCards = GetIntByProbabilityBc.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7);

            if (numberOfNonSupplyCards > availableNonSupplyCards.Count)
            {
                return availableNonSupplyCards;
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableNonSupplyCards, numberOfNonSupplyCards);
        }
    }
}
