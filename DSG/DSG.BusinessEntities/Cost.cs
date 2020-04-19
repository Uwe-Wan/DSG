using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities
{
    [Table("Cost")]
    public class Cost
    {
        public int Id { get; set; }

        [DefaultValue(0)]
        public int Money { get; set; }
        
        [DefaultValue(false)]
        public bool Potion { get; set; }

        [DefaultValue(0)]
        public int Dept { get; set; }
    }
}
