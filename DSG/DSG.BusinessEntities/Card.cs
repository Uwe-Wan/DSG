using System;
using System.Collections.Generic;
using System.Text;

namespace DSG.BusinessEntities
{
    public class Card
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ExpansionId { get; set; }

        public Cost Cost { get; set; }
    }
}
