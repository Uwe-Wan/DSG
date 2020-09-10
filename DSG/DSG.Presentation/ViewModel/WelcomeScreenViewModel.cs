using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

[assembly: InternalsVisibleTo("DSG.Presentation.Test")]

namespace DSG.Presentation.ViewModel
{
    public class WelcomeScreenViewModel : IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;
        private INaviService _naviService;
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

        public INaviService NaviService
        {
            get
            {
                Check.RequireInjected(_naviService, nameof(NaviService), nameof(WelcomeScreenViewModel));
                return _naviService;
            }
            set { _naviService = value; }
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
            NavigateToManageSetsScreenCommand = new RelayCommand(p => NavigateTo(NavigationDestination.ManageSets));
            GenerationOptionsCommand = new RelayCommand(c => GoToGenerationOptions());
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }
        public ICommand GenerationOptionsCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            List<DominionExpansion> expansions = DominionExpansionBc.GetExpansions();
            DominionExpansions.Clear();
            DominionExpansions.AddRange(expansions);
        }

        internal void GoToGenerationOptions()
        {
            int availableCards = DominionExpansions.SelectMany(expansions => expansions.ContainedCards).Count();

            if (availableCards < 10)
            {
                string message = string.Join(" ", "There are only", availableCards, "cards available. A minimum of 10 is needed to generate a set.");
                string caption = "Not enough Cards!";
                UiService.ShowErrorMessage(message, caption);
                return;
            }

            NavigateTo(NavigationDestination.GenerationOptions);
        }

        private void NavigateTo(NavigationDestination destination)
        {
            NaviService.NavigateToAsync(destination, DominionExpansions);
        }
    }
}
