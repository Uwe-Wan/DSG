﻿using DSG.BusinessComponents.Probabilities;
using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
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
                Check.RequireInjected(GetIntByProbabilityBc, nameof(GetIntByProbabilityBc), nameof(SetGeneratorBc));
                return _getIntByProbabilityBc;
            }
            set { _getIntByProbabilityBc = value; }
        }

        public IShuffleListBc<Card> ShuffleListBc
        {
            get
            {
                Check.RequireInjected(ShuffleListBc, nameof(ShuffleListBc), nameof(SetGeneratorBc));
                return _shuffleListBc;
            }
            set { _shuffleListBc = value; }
        }

        public List<Card> GenerateSet(List<DominionExpansion> dominionExpansions)
        {
            List<Card> availableCards = dominionExpansions.SelectMany(expansion => expansion.ContainedCards).ToList();

            List<Card> chosenSupplyCards = ChooseSupplyCards(availableCards);

            List<Card> chosenNonSupplyCards = ChooseNonSupplyCards(availableCards);

            return chosenSupplyCards.Union(chosenNonSupplyCards).ToList();
        }

        private List<Card> ChooseSupplyCards(List<Card> availableCards)
        {
            List<Card> availableSupplyCards = RetrieveCards.SupplyOrOthers(availableCards, true);

            if (availableSupplyCards.Count < 10)
            {
                throw new NotEnoughCardsAvailableException("Not enough Cards available.");
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableSupplyCards, 10);
        }

        private List<Card> ChooseNonSupplyCards(List<Card> availableCards)
        {
            List<Card> availableNonSupplyCards = RetrieveCards.SupplyOrOthers(availableCards, false);

            int numberOfNonSupplyCards = GetIntByProbabilityBc.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7);

            if (numberOfNonSupplyCards > availableNonSupplyCards.Count)
            {
                return availableNonSupplyCards;
            }

            return ShuffleListBc.ReturnGivenNumberOfRandomElementsFromList(availableNonSupplyCards, numberOfNonSupplyCards);
        }

        private List<Card> RetrieveEitherSupplyCardsOrOthers(List<Card> cardsToSplit, bool isSupplyType)
        {
            return cardsToSplit.Where(
                card => card.CardTypeToCards
                    .Select(x => x.CardType.IsSupplyType == isSupplyType)
                    .Any(x => x == true))
                .ToList();
        }
    }
}
