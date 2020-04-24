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
        [Index("UQX_Cost_Money_Dept_Potion", 1, IsUnique = true)]
        public int Money { get; set; }
        
        [DefaultValue(false)]
        [Index("UQX_Cost_Money_Dept_Potion", 2, IsUnique = true)]
        public bool Potion { get; set; }

        [DefaultValue(0)]
        [Index("UQX_Cost_Money_Dept_Potion", 3, IsUnique = true)]
        public int Dept { get; set; }

        public Cost()
        {
        }

        public Cost(int money = 0, int dept = 0, bool potion = false)
        {
            Money = money;
            Dept = dept;
            Potion = potion;
        }

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
