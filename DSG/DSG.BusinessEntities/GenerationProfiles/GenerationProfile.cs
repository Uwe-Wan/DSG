using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.GenerationProfiles
{
    public class GenerationProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("UQX_GenerationProfile_Name", IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public int PropabilityForShelters { get; set; }

        [Required]
        public int PropabilityForPlatinumAndColony { get; set; }

        [Required]
        [ForeignKey(nameof(PropabilityForNonSupplyCards))]
        public int PropabilityForNonSupplyCardsId { get; set; }
        
        public PropabilityForNonSupplyCards PropabilityForNonSupplyCards { get; set; }

        public List<int> SelectedExpansionIds { get; set; }
    }
}
