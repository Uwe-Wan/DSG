using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.StaticMethods
{
    public static class CardHelper
    {
        public static IEnumerable<AdditionalCard> GetAdditionalCardsAlreadyIncluded(this Card card, bool alreadyIncluded)
        {
            return card.CardArtifactsToCard?
                .Select(x => x.CardArtifact)
                .Select(x => x.AdditionalCard)
                .Where(additional => additional?.AlreadyIncludedCard == alreadyIncluded);
        }

        public static bool IsSupplyType(this Card card)
        {
            return IsSupplyType(card, true);
        }

        public static bool IsNonSupplyType(this Card card)
        {
            return IsSupplyType(card, false);
        }

        private static bool IsSupplyType(Card card, bool isSupplyType)
        {
            return card.CardTypeToCards
                    .Where(x => x.CardType.IsSupplyType == isSupplyType)
                    .Any();
        }

        public static List<Card> GetSupplyCards(this IEnumerable<Card> cardsToSplit)
        {
            return GetSupplyOrOthers(cardsToSplit, true);
        }

        public static List<Card> GetNonSupplyCards(this IEnumerable<Card> cardsToSplit)
        {
            return GetSupplyOrOthers(cardsToSplit, false);
        }

        private static List<Card> GetSupplyOrOthers(IEnumerable<Card> cardsToSplit, bool isSupplyType)
        {
            return cardsToSplit
                .Where(card => IsSupplyType(card, isSupplyType))                
                .ToList();
        }

        public static IEnumerable<CardArtifact> GetArtifactsWithoutAdditional(this IEnumerable<Card> cards)
        {
            return cards.SelectMany(card => card.CardArtifactsToCard)
                .Select(x => x.CardArtifact)
                .Where(x => x.AdditionalCard == null);
        }
    }
}
