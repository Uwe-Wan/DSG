using DSG.BusinessComponents.Generation;
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
        private GenerationParameterDto _generationParameter;

        public IUiService UiService
        {
            get
            {
                Check.RequireInjected(_uiService, nameof(UiService), nameof(GenerationOptionsViewModel));
                return _uiService;
            }
            set { _uiService = value; }
        }

        public GenerationParameterDto GenerationParameter
        {
            get { return _generationParameter; }
            set
            {
                _generationParameter = value;
                OnPropertyChanged(nameof(GenerationParameter));
            }
        }

        public GenerationOptionsViewModel()
        {
            GenerateSetCommand = new RelayCommand(c => GenerateSet());
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
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();

            GenerationParameter = new GenerationParameterDto(
                isDominionExpansionSelectedDtos, 
                20, 
                SetupInitialPropabilitiesForNonSupplies());
        }

        private Dictionary<int, int> SetupInitialPropabilitiesForNonSupplies()
        {
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 50);
            propabilitiesForNonSuppliesByAmount.Add(2, 30);
            propabilitiesForNonSuppliesByAmount.Add(3, 7);
            propabilitiesForNonSuppliesByAmount.Add(4, 0);

            return propabilitiesForNonSuppliesByAmount;
        }

        internal void GenerateSet()
        {
            List<Card> availableCards = GenerationParameter.IsDominionExpansionSelectedDtos
                .Where(dto => dto.IsSelected)
                .SelectMany(dto => dto.DominionExpansion.ContainedCards)
                .ToList();
            int availableSupplyCards = CardHelper.GetSupplyCards(availableCards).Count;

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
            NaviService.NavigateToAsync(destination, GenerationParameter);
        }

        #endregion Methods
    }
}
