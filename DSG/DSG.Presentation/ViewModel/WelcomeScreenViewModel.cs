using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class WelcomeScreenViewModel : IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;
        private INaviService _naviService;

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

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public WelcomeScreenViewModel()
        {
            NavigateToManageSetsScreenCommand = new RelayCommand(p => NavigateTo(NavigationDestination.ManageSets));
            GenerateSetCommand = new RelayCommand(p => NavigateTo(NavigationDestination.GeneratedSet));
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

        private void NavigateTo(NavigationDestination destination)
        {
            NaviService.NavigateToAsync(destination, DominionExpansions);
        }
    }
}
