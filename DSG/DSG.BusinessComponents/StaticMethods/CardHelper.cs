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
                .Where(additional => additional.AlreadyIncludedCard == alreadyIncluded);
        }
    }
}
