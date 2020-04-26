using DSG.BusinessEntities;
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

            if(availableCards.Count < 10)
            {
                throw new Exception("Not enough Cards available.");
            }

            Random random = new Random();
            return availableCards.OrderBy(x => random.Next()).Take(10).ToList();
        }
    }
}
