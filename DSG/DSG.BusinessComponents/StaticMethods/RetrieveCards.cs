using DSG.BusinessEntities;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.StaticMethods
{
    public static class RetrieveCards
    {
        public static List<Card> SupplyOrOthers(List<Card> cardsToSplit, bool isSupplyType)
        {
            return cardsToSplit.Where(
                card => card.CardTypeToCards
                    .Select(x => x.CardType.IsSupplyType == isSupplyType)
                    .Any(x => x == true))
                .ToList();
        }
    }
}
