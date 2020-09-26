using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Generation
{
    public class GeneratedSetViewModel : AbstractViewModel, IViewModel
    {
        private ISetGeneratorBc _setGeneratorBc;
        private List<Card> _generatedSet;

        public List<Card> GeneratedSet
        {
            get { return _generatedSet; }
            set
            {
                _generatedSet = value;
                OnPropertyChanged(nameof(GeneratedSet));
            }
        }

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
            NavigateToGenerationOptionsCommand = new RelayCommand(async c => await NavigateToAsync(NavigationDestination.GenerationOptions));
        }

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            IsDataLoaded = false;
            await Task.Run(() => LoadData(data[0]));
            IsDataLoaded = true;
        }

        private void LoadData(object expansionsData)
        {
            List<DominionExpansion> expansions = expansionsData as List<DominionExpansion>;
            GeneratedSet = SetGeneratorBc.GenerateSet(expansions);
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            GeneratedSet = null;
        }
    }
}
