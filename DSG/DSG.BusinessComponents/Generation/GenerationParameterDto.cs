using DSG.BusinessEntities;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponents.Generation
{
    public class GenerationParameterDto
    {
        public List<DominionExpansion> Expansions => IsDominionExpansionSelectedDtos
                .Where(dto => dto.IsSelected == true)
                .Select(dto => dto.DominionExpansion)
                .ToList();

        public int PropabilityForColonyAndPlatinum { get; set; }

        public int PropabilityForShelters { get; set; }

        public Dictionary<int, int> PropabilitiesForNonSuppliesByAmount { get; set; }

        public List<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationParameterDto(
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos, 
            int propabilityForPlatinumAndColony,
            int propabilityForShelters,
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount)
        {
            IsDominionExpansionSelectedDtos = isDominionExpansionSelectedDtos;
            PropabilityForColonyAndPlatinum = propabilityForPlatinumAndColony;
            PropabilityForShelters = propabilityForShelters;
            PropabilitiesForNonSuppliesByAmount = propabilitiesForNonSuppliesByAmount;
        }
    }
}
