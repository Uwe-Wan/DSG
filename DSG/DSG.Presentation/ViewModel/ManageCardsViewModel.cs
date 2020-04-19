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
    public class ManageCardsViewModel : Notifier, IViewModel
    {
        private Card _selectedCard;

        public DominionExpansion SelectedExpansion { get; set; }

        public Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public string UserInput { get; set; }

        public ManageCardsViewModel()
        {
        }

        public async Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data)
        {
            SelectedExpansion = (DominionExpansion)data[0];
        }
    }
}
