using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class WelcomeScreenViewModel : IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;
        private INaviService _naviService;
        private IUiService _uiService;

        public IDominionExpansionBc DominionExpansionBc
        {
            get { return _dominionExpansionBc; }
            set { _dominionExpansionBc = value; }
        }

        public INaviService NaviService
        {
            get { return _naviService; }
            set { _naviService = value; }
        }

        public IUiService UiService
        {
            get { return _uiService; }
            set { _uiService = value; }
        }

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public WelcomeScreenViewModel()
        {
            NavigateToManageSetsScreenCommand = new RelayCommand(p => NavigateTo(NavigationDestination.ManageSets));
            GenerateSetCommand = new RelayCommand(p => GenerateSet());
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }
        public ICommand GenerateSetCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            List<DominionExpansion> expansions = DominionExpansionBc.GetExpansions();
            DominionExpansions.Clear();
            DominionExpansions.AddRange(expansions);
        }

        private void GenerateSet()
        {
            int availableCards = DominionExpansions.SelectMany(expansions => expansions.ContainedCards).Count();
            
            if(availableCards < 10)
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
