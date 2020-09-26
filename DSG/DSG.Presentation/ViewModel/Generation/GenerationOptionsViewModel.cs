using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.Common;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace DSG.Presentation.ViewModel.Generation
{
    public class GenerationOptionsViewModel : AbstractViewModel, IViewModel
    {
        private IUiService _uiService;

        public IUiService UiService
        {
            get
            {
                Check.RequireInjected(_uiService, nameof(UiService), nameof(GenerationOptionsViewModel));
                return _uiService;
            }
            set { _uiService = value; }
        }

        public List<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationOptionsViewModel()
        {
            GenerateSetCommand = new RelayCommand(c => GenerateSet());

            IsDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto>();
        }

        public ICommand GenerateSetCommand { get; private set; }


        #region Methods

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            if(data.Length == 0)
            {
                return;
            }

            IsDataLoaded = false;
            await Task.Run(() => LoadData(data[0]));
            IsDataLoaded = true;
        }

        private void LoadData(object expansionsData)
        {
            IEnumerable<DominionExpansion> expansions = expansionsData as IEnumerable<DominionExpansion>;

            IsDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();
        }

        internal void GenerateSet()
        {
            List<Card> availableCards = IsDominionExpansionSelectedDtos
                .Where(dto => dto.IsSelected)
                .SelectMany(dto => dto.DominionExpansion.ContainedCards)
                .ToList();
            int availableSupplyCards = RetrieveCards.SupplyOrOthers(availableCards, true).Count;

            if (availableSupplyCards < 10)
            {
                string message = string.Join(" ", "There are only", availableSupplyCards, "cards available in the selected Sets. " +
                    "A minimum of 10 is needed to generate a set.");
                string caption = "Not enough Cards!";
                UiService.ShowErrorMessage(message, caption);
                return;
            }

            NavigateTo(NavigationDestination.GeneratedSet);
        }

        private void NavigateTo(NavigationDestination destination)
        {
            List<DominionExpansion> selectedExpansions = IsDominionExpansionSelectedDtos
                .Where(dto => dto.IsSelected == true)
                .Select(dto => dto.DominionExpansion)
                .ToList();
            NaviService.NavigateToAsync(destination, selectedExpansions);
        }

        #endregion Methods
    }
}
