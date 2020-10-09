using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.StaticMethods
{
    public static class CardHelper
    {
        public static IEnumerable<AdditionalCard> GetAdditionalCardsAlreadyIncluded(Card card, bool alreadyIncluded)
        {
            return card.CardArtifactsToCard?
                .Select(x => x.CardArtifact)
                .Select(x => x.AdditionalCard)
                .Where(additional => additional?.AlreadyIncludedCard == alreadyIncluded);
        }

        public static bool IsSupplyType(Card card)
        {
            return IsSupplyType(card, true);
        }

        public static bool IsNonSupplyType(Card card)
        {
            return IsSupplyType(card, false);
        }

        private static bool IsSupplyType(Card card, bool isSupplyType)
        {
            return card.CardTypeToCards
                    .Select(x => x.CardType.IsSupplyType == isSupplyType)
                    .Any(x => x == true);
        }

        public static List<Card> GetSupplyCards(List<Card> cardsToSplit)
        {
            return GetSupplyOrOthers(cardsToSplit, true);
        }

        public static List<Card> GetNonSupplyCards(List<Card> cardsToSplit)
        {
            return GetSupplyOrOthers(cardsToSplit, false);
        }

        private static List<Card> GetSupplyOrOthers(List<Card> cardsToSplit, bool isSupplyType)
        {
            return cardsToSplit
                .Where(card => IsSupplyType(card, isSupplyType))                
                .ToList();
        }
    }
}
