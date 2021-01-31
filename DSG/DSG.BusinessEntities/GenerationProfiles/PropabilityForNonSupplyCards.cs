using System.ComponentModel.DataAnnotations;

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
    }
}
