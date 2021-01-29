using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace DSG.Presentation.ViewModel.Generation
{
    public class GenerationOptionsViewModel : AbstractViewModel, IViewModel
    {
        private IUiService _uiService;
        private IGenerationProfileBc _generationProfileBc;
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

        public IGenerationProfileBc GenerationProfileBc
        {
            get
            {
                Check.RequireInjected(_uiService, nameof(GenerationProfileBc), nameof(GenerationOptionsViewModel));
                return _generationProfileBc;
            }
            set { _generationProfileBc = value; }
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

        public ObservableCollection<GenerationProfile> GenerationProfiles { get; set; }

        public GenerationProfile SelectedProfile { get; set; }

        public ObservableCollection<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationOptionsViewModel()
        {
            GenerateSetCommand = new RelayCommand(c => GenerateSet());
            GenerationProfiles = new ObservableCollection<GenerationProfile>();
            IsDominionExpansionSelectedDtos = new ObservableCollection<IsDominionExpansionSelectedDto>();
        }

        public ICommand GenerateSetCommand { get; private set; }


        #region Methods

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            if (data.Length == 0)
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

            SetupInitialGenerationProfile();

            AddNewExpansionsToSelection(expansions);

            LoadInitialGenerationProfiles();
        }

        private void AddNewExpansionsToSelection(IEnumerable<DominionExpansion> expansions)
        {
            List<int> availableExpansionIds = IsDominionExpansionSelectedDtos.Select(x => x.DominionExpansion.Id).ToList();
            IEnumerable<DominionExpansion> newExpansions = expansions.Where(expansion => availableExpansionIds.Contains(expansion.Id) == false);
            IsDominionExpansionSelectedDtos.AddRange(newExpansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)));
        }

        private void LoadInitialGenerationProfiles()
        {
            if(GenerationProfiles.Count == 0)
            {
                GenerationProfiles.AddRange(GenerationProfileBc.GetGenerationProfiles());
            }
        }

        private void SetupInitialGenerationProfile()
        {
            if (SelectedProfile == null)
            {
                SelectedProfile = new GenerationProfile(
                    10,
                    20,
                    SetupInitialPropabilitiesForNonSupplies());
            }
        }

        private PropabilityForNonSupplyCards SetupInitialPropabilitiesForNonSupplies()
        {
            PropabilityForNonSupplyCards propabilityForNonSupplyCards = new PropabilityForNonSupplyCards();
            propabilityForNonSupplyCards.PropabilityForOne = 50;
            propabilityForNonSupplyCards.PropabilityForTwo = 30;
            propabilityForNonSupplyCards.PropabilityForThree = 7;
            propabilityForNonSupplyCards.PropabilityForFour = 0;

            return propabilityForNonSupplyCards;
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
