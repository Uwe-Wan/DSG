using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Generation
{
    public class GeneratedSetViewModel : AbstractViewModel, IViewModel
    {
        private ISetGeneratorBc _setGeneratorBc;

        public List<Card> GeneratedSet { get; set; }

        public ISetGeneratorBc SetGeneratorBc
        {
            get
            {
                Check.RequireInjected(_setGeneratorBc, nameof(SetGeneratorBc), nameof(GeneratedSetViewModel));
                return _setGeneratorBc;
            }
            set { _setGeneratorBc = value; }
        }


        public ICommand NavigateToGenerationOptionsCommand { get; private set; }

        public GeneratedSetViewModel()
        {
            NavigateToGenerationOptionsCommand = new RelayCommand(c => NaviService.NavigateToAsync(NavigationDestination.GenerationOptions));
        }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            IEnumerable<DominionExpansion> expansionData = data[0] as IEnumerable<DominionExpansion>;
            List<DominionExpansion> expansions = expansionData.ToList();

            GeneratedSet = SetGeneratorBc.GenerateSet(expansions);
        }
    }
}
