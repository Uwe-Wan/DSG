using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessEntities
{
    [Table("Cost")]
    public class Cost
    {
        public int Id { get; set; }

        public int? Money { get; set; }

        public int? Potion { get; set; }

        public int? Dept { get; set; }
    }
}
