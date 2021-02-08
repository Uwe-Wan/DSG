using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.GenerationProfiles
{
    public class GenerationProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
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

        public List<SelectedExpansionToGenerationProfile> SelectedExpansions { get; set; }

        public GenerationProfile()
        {
            SelectedExpansions = new List<SelectedExpansionToGenerationProfile>();
        }

        public GenerationProfile(int propabilitiesForShelters, int propabilityForColonyAndPlatinum, PropabilityForNonSupplyCards propabilityForNonSupplyCards)
        {
            PropabilityForShelters = propabilitiesForShelters;
            PropabilityForPlatinumAndColony = propabilityForColonyAndPlatinum;
            PropabilityForNonSupplyCards = propabilityForNonSupplyCards;
            SelectedExpansions = new List<SelectedExpansionToGenerationProfile>();
        }

        public GenerationProfile Clone()
        {
            GenerationProfile newProfile = new GenerationProfile(PropabilityForShelters, PropabilityForPlatinumAndColony, PropabilityForNonSupplyCards.Clone());
            newProfile.Name = Name;
            newProfile.SelectedExpansions = SelectedExpansions;

            return newProfile;
        }
    }
}
