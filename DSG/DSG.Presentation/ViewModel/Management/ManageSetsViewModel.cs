using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DSG.Common.Extensions;
using DSG.Common;

namespace DSG.Presentation.ViewModel.Management
{
    public class ManageSetsViewModel : AbstractViewModel
    {
        #region Fields

        private IDominionExpansionBc _dominionExpansionBc;
        private DominionExpansion _selectedExpansion;

        #endregion Fields


        #region Injected Dependencies

        public IDominionExpansionBc DominionExpansionBc
        {
            get
            {
                Check.RequireInjected(_dominionExpansionBc, nameof(DominionExpansionBc), nameof(ManageSetsViewModel));
                return _dominionExpansionBc;
            }
            set { _dominionExpansionBc = value; }
        }

        #endregion Injected Dependencies


        #region Properties

        public DominionExpansion SelectedExpansion
        {
            get { return _selectedExpansion; }
            set
            {
                _selectedExpansion = value;
                OnPropertyChanged(nameof(SelectedExpansion));
            }
        }

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public string NewSetsName { get; set; }

        #endregion Properties


        public ManageSetsViewModel()
        {
            InsertCommand = new RelayCommand(c => InsertExpansion());
            AddCardsCommand = new RelayCommand(async c => await NavigateToAsync(NavigationDestination.ManageCards));
            AddCardArtifactsCommand = new RelayCommand(async c => await NavigateToAsync(NavigationDestination.ManageCardArtifacts));
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        #region Commands

        public ICommand InsertCommand { get; private set; }
        public ICommand AddCardsCommand { get; private set; }
        public ICommand AddCardArtifactsCommand { get; private set; }

        #endregion Commands


        #region Methods

#pragma warning disable CS1998 // Async method lacks 'await' operators, needs to be async because is inherited but no need to make this really asynchronous
        public override async Task OnPageLoadedAsync(params object[] data)
#pragma warning restore CS1998
        {
            if (data.Length == 0)
            {
                return;
            }

            IsDataLoaded = false;

            DominionExpansions.Clear();

            IEnumerable<DominionExpansion> expansions = data[0] as IEnumerable<DominionExpansion>;
            DominionExpansions.AddRange(expansions);

            IsDataLoaded = true;
        }

        public void InsertExpansion()
        {
            Check.RequireNotNullNotEmpty(NewSetsName, nameof(NewSetsName), nameof(ManageSetsViewModel));

            DominionExpansionBc.InsertExpansion(NewSetsName);
            DominionExpansion newExpansion = DominionExpansionBc.GetExpansionByName(NewSetsName);
            DominionExpansions.Add(newExpansion);
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await NaviService.NavigateToAsync(destination, SelectedExpansion, DominionExpansions);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            NewSetsName = null;
        }

        #endregion Methods
    }
}
