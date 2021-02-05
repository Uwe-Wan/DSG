using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Common;
using DSG.Common.Extensions;
using DSG.Presentation.Messaging;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using DSG.Validation.GenerationProfiles;
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
        private GenerationProfile _selectedProfile;
        private IMessenger _messenger;

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

        public IMessenger Messenger
        {
            get
            {
                Check.RequireInjected(_messenger, nameof(Messenger), nameof(GenerationOptionsViewModel));
                return _messenger;
            }
            set { _messenger = value; }
        }

        public ObservableCollection<GenerationProfileViewEntity> GenerationProfiles { get; set; }

        public GenerationProfile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }
            set
            {
                _selectedProfile = value;
                OnPropertyChanged(nameof(SelectedProfile));
            }
        }

        public ObservableCollection<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationOptionsViewModel(IMessenger messenger)
        {
            GenerateSetCommand = new RelayCommand(c => GenerateSet());
            SaveProfileCommand = new RelayCommand(c => SaveProfile());

            GenerationProfiles = new ObservableCollection<GenerationProfileViewEntity>();
            IsDominionExpansionSelectedDtos = new ObservableCollection<IsDominionExpansionSelectedDto>();

            Messenger = messenger;
            Messenger.Register(Message.ProfileLoaded, LoadProfile);
        }

        public ICommand GenerateSetCommand { get; private set; }
        public ICommand SaveProfileCommand { get; private set; }


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

            AddNewExpansionsToSelection(expansions);

            SetupInitialGenerationProfile();

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
            if (GenerationProfiles.Count == 0)
            {
                List<GenerationProfileViewEntity> loadedProfiles = GenerationProfileBc.GetGenerationProfiles()
                    .Select(profile => new GenerationProfileViewEntity(profile, Messenger, IsDominionExpansionSelectedDtos))
                    .ToList();
                GenerationProfiles.AddRange(loadedProfiles);
            }
        }

        private void SetupInitialGenerationProfile()
        {
            if (SelectedProfile == null)
            {
                SelectedProfile = new GenerationProfile(
                    10,
                    20,
                    SetupInitialPropabilitiesForNonSupplies())
                {
                    Name = "Insert Name"
                };
            }
        }

        private PropabilityForNonSupplyCards SetupInitialPropabilitiesForNonSupplies()
        {
            PropabilityForNonSupplyCards propabilityForNonSupplyCards = new PropabilityForNonSupplyCards(50, 30, 7, 0);

            return propabilityForNonSupplyCards;
        }

        internal void GenerateSet()
        {
            List<Card> availableCards = IsDominionExpansionSelectedDtos
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

        internal void SaveProfile()
        {
            GenerationProfile newProfile = SelectedProfile.Clone();
            newProfile.SelectedExpansions = IsDominionExpansionSelectedDtos
                .Where(x => x.IsSelected)
                .Select(x => new SelectedExpansionToGenerationProfile(x.DominionExpansion))
                .ToList();
            GenerationProfileBc.InsertGenerationProfile(newProfile);

            GenerationProfileViewEntity newProfileViewEntity = new GenerationProfileViewEntity(newProfile, Messenger, IsDominionExpansionSelectedDtos);

            GenerationProfiles.Add(newProfileViewEntity);
        }

        internal void LoadProfile(object data)
        {
            GenerationProfile profile = (GenerationProfile)data;
            SelectedProfile = profile.Clone();
            SelectedProfile.Name = "Insert Name";
        }

        private void NavigateTo(NavigationDestination destination)
        {
            GenerationParameterDto generationParameter = new GenerationParameterDto(IsDominionExpansionSelectedDtos, SelectedProfile);
            NaviService.NavigateToAsync(destination, generationParameter);
        }

        #endregion Methods
    }
}
