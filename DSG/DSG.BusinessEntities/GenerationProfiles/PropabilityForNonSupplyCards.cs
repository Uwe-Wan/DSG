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
    }
}
