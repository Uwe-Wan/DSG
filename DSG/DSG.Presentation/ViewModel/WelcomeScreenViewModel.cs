using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace DSG.Presentation.ViewModel
{
    public class WelcomeScreenViewModel : AbstractViewModel, IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;
        private IUiService _uiService;

        public IDominionExpansionBc DominionExpansionBc
        {
            get
            {
                Check.RequireInjected(_dominionExpansionBc, nameof(DominionExpansionBc), nameof(WelcomeScreenViewModel));
                return _dominionExpansionBc;
            }
            set { _dominionExpansionBc = value; }
        }

        public IUiService UiService
        {
            get
            {
                Check.RequireInjected(_uiService, nameof(UiService), nameof(WelcomeScreenViewModel));
                return _uiService;
            }
            set { _uiService = value; }
        }

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public WelcomeScreenViewModel()
        {
            NavigateToManageSetsScreenCommand = new RelayCommand(async p => await NavigateToAsync(NavigationDestination.ManageSets));
            GenerationOptionsCommand = new RelayCommand(async c => await GoToGenerationOptions());
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }
        public ICommand GenerationOptionsCommand { get; private set; }


        public override async Task OnPageLoadedAsync(params object[] data)
        {
            IsDataLoaded = false;
            Task<List<DominionExpansion>> getExpansionsTask = Task.Run(DominionExpansionBc.GetExpansions);
            await getExpansionsTask;
            DominionExpansions.AddRange(getExpansionsTask.Result);
            IsDataLoaded = true;
        }

        internal async Task GoToGenerationOptions()
        {
            int availableCards = DominionExpansions.SelectMany(expansions => expansions.ContainedCards).Count();

            if (availableCards < 10)
            {
                string message = string.Join(" ", "There are only", availableCards, "cards available. A minimum of 10 is needed to generate a set.");
                string caption = "Not enough Cards!";
                UiService.ShowErrorMessage(message, caption);
                return;
            }

            await NavigateToAsync(NavigationDestination.GenerationOptions);
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await NaviService.NavigateToAsync(destination, DominionExpansions);
            await Task.Run(CleanData);
        }

        private void CleanData()
        {
            DominionExpansions.Clear();
        }
    }
}
