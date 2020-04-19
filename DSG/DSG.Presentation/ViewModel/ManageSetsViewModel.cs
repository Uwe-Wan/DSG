using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DSG.Common.Extensions;

namespace DSG.Presentation.ViewModel
{
    public class ManageSetsViewModel : IViewModel
    {
        private IDominionExpansionBc _dominionExpansionBc;

        public IDominionExpansionBc DominionExpansionBc
        {
            get { return _dominionExpansionBc; }
            set { _dominionExpansionBc = value; }
        }

        public DominionExpansion SelectedExpansion { get; set; }

        public ObservableCollection<DominionExpansion> DominionExpansions { get; set; }

        public string UserInput { get; set; }

        public ManageSetsViewModel()
        {
            InsertCommand = new RelayCommand(p => InsertExpansion());
            DominionExpansions = new ObservableCollection<DominionExpansion>();
        }

        public ICommand InsertCommand { get; set; }

        public async Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data)
        {
            List<DominionExpansion> expansions = DominionExpansionBc.GetExpansions();
            DominionExpansions.Clear();
            DominionExpansions.AddRange(expansions);
        }

        public void InsertExpansion()
        {
            //todo: add a check.requireNotNull here

            DominionExpansionBc.InsertExpansion(UserInput);
            DominionExpansion newExpansion = DominionExpansionBc.GetExpansionByName(UserInput);
            DominionExpansions.Add(newExpansion);
        }
    }
}
