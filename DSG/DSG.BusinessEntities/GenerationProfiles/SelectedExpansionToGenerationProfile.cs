using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.GenerationProfiles
{
    public class SelectedExpansionToGenerationProfile
    {
        [Key, Column(Order = 1)]
        [ForeignKey(nameof(DominionExpansion))]
        public int DominionExpansionId { get; set; }

        public DominionExpansion DominionExpansion { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey(nameof(GenerationProfile))]
        public int GenerationProfileId { get; set; }

        public GenerationProfile GenerationProfile { get; set; }
    }
}
