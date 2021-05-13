using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DSG.BusinessComponents.Generation
{
    public class GenerationParameterDto
    {
        public Dictionary<DominionExpansion, int> WeightsByExpansions { get; set; }

        public int PropabilityForColonyAndPlatinum { get; set; }

        public int PropabilityForShelters { get; set; }

        public PropabilityForNonSupplyCards PropabilitiesForNonSupplies { get; set; }

        public GenerationParameterDto(
            ObservableCollection<IsSelectedAndWeightedExpansionDto> isDominionExpansionSelectedDtos, 
            GenerationProfile generationProfile)
        {
            WeightsByExpansions = isDominionExpansionSelectedDtos.Where(x => x.IsSelected).ToDictionary(dto => dto.DominionExpansion, dto => dto.Weight);
            PropabilityForColonyAndPlatinum = generationProfile.PropabilityForPlatinumAndColony;
            PropabilityForShelters = generationProfile.PropabilityForShelters;
            PropabilitiesForNonSupplies = generationProfile.PropabilityForNonSupplyCards;
        }
    }
}
