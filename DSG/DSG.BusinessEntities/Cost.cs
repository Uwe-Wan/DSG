using System;
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Cost cost = (Cost)obj;
            if(cost.Money == this.Money && cost.Dept == this.Dept && cost.Potion == this.Potion)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() << 2) ^ Money;
        }
    }
}
