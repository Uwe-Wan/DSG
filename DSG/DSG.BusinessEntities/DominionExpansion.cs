using System;
using System.Collections.Generic;

namespace DSG.BusinessEntities
{
    public class DominionExpansion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Card> ContainedCards { get; set; }
    }
}
