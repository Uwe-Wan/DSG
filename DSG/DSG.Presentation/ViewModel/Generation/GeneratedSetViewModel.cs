using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSG.Presentation.ViewModel.Generation
{
    public class GeneratedSetViewModel : IViewModel
    {
        private ISetGeneratorBc _setGeneratorBc;

        public List<Card> GeneratedSet { get; set; }

        public ISetGeneratorBc SetGeneratorBc
        {
            get
            {
                Check.RequireInjected(SetGeneratorBc, nameof(SetGeneratorBc), nameof(GeneratedSetViewModel));
                return _setGeneratorBc;
            }
            set { _setGeneratorBc = value; }
        }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            IEnumerable<DominionExpansion> expansionData = data[0] as IEnumerable<DominionExpansion>;
            List<DominionExpansion> expansions = expansionData.ToList();

            GeneratedSet = SetGeneratorBc.GenerateSet(expansions);
        }
    }
}
