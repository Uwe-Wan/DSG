using DSG.BusinessEntities;
using System.Collections.Generic;

namespace DSG.BusinessComponents.Generation
{
    public class GenerationParameterDto
    {
        public List<DominionExpansion> Expansions { get; set; }

        public int PropabilityForColonyAndPlatinum { get; set; }

        public GenerationParameterDto(List<DominionExpansion> expansions, int propabilityForPlatinumAndColony)
        {
            Expansions = expansions;
            PropabilityForColonyAndPlatinum = propabilityForPlatinumAndColony;
        }
    }
}
