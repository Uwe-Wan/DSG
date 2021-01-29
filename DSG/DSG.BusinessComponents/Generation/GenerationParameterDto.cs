using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DSG.BusinessComponents.Generation
{
    public class GenerationParameterDto
    {
        public List<DominionExpansion> Expansions { get; set; }

        public int PropabilityForColonyAndPlatinum { get; set; }

        public int PropabilityForShelters { get; set; }

        public PropabilityForNonSupplyCards PropabilitiesForNonSuppliesByAmount { get; set; }

        public GenerationParameterDto(
            ObservableCollection<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos, 
            GenerationProfile generationProfile)
        {
            Expansions = isDominionExpansionSelectedDtos.Where(x => x.IsSelected).Select(x => x.DominionExpansion).ToList();
            PropabilityForColonyAndPlatinum = generationProfile.PropabilityForPlatinumAndColony;
            PropabilityForShelters = generationProfile.PropabilityForShelters;
            PropabilitiesForNonSuppliesByAmount = generationProfile.PropabilityForNonSupplyCards;
        }
    }
}
