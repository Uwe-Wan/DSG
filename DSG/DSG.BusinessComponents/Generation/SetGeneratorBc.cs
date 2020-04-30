using DSG.BusinessEntities;
using DSG.BusinessEntities.GetEnums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.Generation
{
    public class SetGeneratorBc : ISetGeneratorBc
    {
        public List<Card> GenerateSet(List<DominionExpansion> dominionExpansions)
        {
            List<Card> availableCards = dominionExpansions.SelectMany(expansion => expansion.ContainedCards).ToList();

            List<CardTypeEnum> cardTypes = CardTypeGetter.GetEnum();

            List<Card> availableKingdomCards = availableCards.Where(
                card => card.CardTypeToCards
                    .Select(x => x.CardType.IsKingdomCard)
                    .Any(x => x == true))
                .ToList();

            if(availableKingdomCards.Count < 10)
            {
                throw new Exception("Not enough Cards available.");
            }

            Random random = new Random();
            return availableKingdomCards.OrderBy(x => random.Next()).Take(10).ToList();
        }
    }
}
