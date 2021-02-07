using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace DSG.BusinessEntities.GenerationProfiles
{
    public class PropabilityForNonSupplyCards
    {
        public int Id { get; set; }

        [Required]
        public int PropabilityForOne { get; set; }

        [Required]
        public int PropabilityForTwo { get; set; }

        [Required]
        public int PropabilityForThree { get; set; }

        [Required]
        public int PropabilityForFour { get; set; }

        public PropabilityForNonSupplyCards()
        {
        }

        public PropabilityForNonSupplyCards(int one, int two, int three, int four)
        {
            PropabilityForOne = one;
            PropabilityForTwo = two;
            PropabilityForThree = three;
            PropabilityForFour = four;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj == DependencyProperty.UnsetValue)
            {
                return false;
            }

            PropabilityForNonSupplyCards prop = (PropabilityForNonSupplyCards)obj;
            if (prop.PropabilityForOne == this.PropabilityForOne 
                && prop.PropabilityForTwo == this.PropabilityForTwo 
                && prop.PropabilityForThree == this.PropabilityForThree
                && prop.PropabilityForFour == this.PropabilityForFour)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = (base.GetHashCode() << 2) ^ PropabilityForOne.GetHashCode();
            hashCode = (hashCode << 2) ^ PropabilityForTwo.GetHashCode();
            hashCode = (hashCode << 2) ^ PropabilityForThree.GetHashCode();
            hashCode = (hashCode << 2) ^ PropabilityForFour.GetHashCode();

            return hashCode;
        }
    }
}
