using DSG.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.Generation
{
    public class SetGeneratorBc : ISetGeneratorBc
    {
        public List<Card> GenerateSet(List<Card> availableCards)
        {
            if(availableCards.Count < 10)
            {
                return null;
            }

            Random random = new Random();
            return availableCards.OrderBy(x => random.Next()).Take(10).ToList();
        }
    }
}
