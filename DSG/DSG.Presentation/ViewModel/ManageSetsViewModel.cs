using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public List<DominionExpansion> DominionExpansions { get; set; }

        public string UserInput { get; set; }

        public ManageSetsViewModel()
        {
            InsertCommand = new RelayCommand(p => InsertExpansion());
        }

        public ICommand InsertCommand { get; set; }

        public async Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data)
        {
            DominionExpansions = DominionExpansionBc.GetExpansions();
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
