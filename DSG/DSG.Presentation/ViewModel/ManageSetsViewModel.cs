using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DSG.Common.Extensions;
using DSG.Common;

namespace DSG.Presentation.ViewModel
{
    public class ManageSetsViewModel : IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;
        private INaviService _naviService;

        public IDominionExpansionBc DominionExpansionBc
        {
            get
            {
                Check.RequireInjected(DominionExpansionBc, nameof(DominionExpansionBc), nameof(ManageSetsViewModel));
                return _dominionExpansionBc;
            }
            set { _dominionExpansionBc = value; }
        }

        public INaviService NaviService
        {
            get
            {
                Check.RequireInjected(NaviService, nameof(NaviService), nameof(ManageSetsViewModel));
                return _naviService;
            }
            set { _naviService = value; }
        }

        public DominionExpansion SelectedExpansion { get; set; }

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public string UserInput { get; set; }

        public ManageSetsViewModel()
        {
            InsertCommand = new RelayCommand(c => InsertExpansion());
            AddCardsCommand = new RelayCommand(c => AddCards());
            AddCardArtifactsCommand = new RelayCommand(c => AddCardArtifacts());
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        public ICommand InsertCommand { get; private set; }
        public ICommand AddCardsCommand { get; private set; }
        public ICommand AddCardArtifactsCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            DominionExpansions.Clear();

            IEnumerable<DominionExpansion> expansions = data[0] as IEnumerable<DominionExpansion>;

            DominionExpansions.AddRange(expansions);
        }

        public void InsertExpansion()
        {
            Check.RequireNotNullNotEmpty(UserInput, nameof(UserInput), nameof(ManageSetsViewModel));

            DominionExpansionBc.InsertExpansion(UserInput);
            DominionExpansion newExpansion = DominionExpansionBc.GetExpansionByName(UserInput);
            DominionExpansions.Add(newExpansion);
        }

        public void AddCards()
        {
            NaviService.NavigateToAsync(NavigationDestination.ManageCards, SelectedExpansion, DominionExpansions);
        }

        public void AddCardArtifacts()
        {
            NaviService.NavigateToAsync(NavigationDestination.ManageCardArtifacts, SelectedExpansion, DominionExpansions);
        }
    }
}
