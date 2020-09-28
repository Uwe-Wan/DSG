using DSG.BusinessComponents.CardArtifacts;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Management
{
    public class ManageCardArtifactViewModel : AbstractViewModel
    {
        #region Fields 

        private ICardArtifactBc _cardArtifactBc;
        private int? _selectedAmountOfAdditionalCards;
        public string _nameOfNewArtifact;
        private string _manageCardArtifactsScreenTitle;
        private SelectedExpansionViewEntity _selectedExpansionViewEntity;
        private CardArtifact _newArtifact;

        #endregion Fields


        #region Injected Dependencies

        public ICardArtifactBc CardArtifactBc
        {
            get
            {
                Check.RequireInjected(_cardArtifactBc, nameof(CardArtifactBc), nameof(ManageCardArtifactViewModel));
                return _cardArtifactBc;
            }
            set { _cardArtifactBc = value; }
        }

        #endregion Injected Dependencies


        #region Properties

        public string ManageCardArtifactsScreenTitle
        {
            get { return _manageCardArtifactsScreenTitle ?? "Manage Artifacts of Expansion ???"; }
            set
            {
                _manageCardArtifactsScreenTitle = value;
                OnPropertyChanged(nameof(ManageCardArtifactsScreenTitle));
            }
        }

        public SelectedExpansionViewEntity SelectedExpansionViewEntity
        {
            get { return _selectedExpansionViewEntity; }
            set
            {
                _selectedExpansionViewEntity = value;
                OnPropertyChanged(nameof(SelectedExpansionViewEntity));
            }
        }

        public ObservableCollection<AdditionalCard> AdditionalCards { get; set; }

        public List<TypeOfAdditionalCard> AvailableTypesOfAdditionalCard { get; set; }

        public TypeOfAdditionalCard SelectedAdditionalCardType { get; set; }

        public CardArtifact NewArtifact
        {
            get { return _newArtifact; }
            set
            {
                _newArtifact = value;
                OnPropertyChanged(nameof(NewArtifact));
            }
        }

        #endregion Properties


        public ManageCardArtifactViewModel()
        {
            AddArtifactCommand = new RelayCommand(c => AddArtifact());
            AdditionalCards = new ObservableCollection<AdditionalCard>();
            AvailableTypesOfAdditionalCard = Enum.GetValues(typeof(TypeOfAdditionalCard))
                    .OfType<TypeOfAdditionalCard>()
                    .ToList();

            NavigateToManageSetsScreenCommand = new RelayCommand(async cmd => await NavigateToAsync(NavigationDestination.ManageSets));
        }


        #region Commands

        public ICommand AddArtifactCommand { get; private set; }
        public ICommand NavigateToManageSetsScreenCommand { get; private set; }

        #endregion Commands


        #region Methods

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            IsDataLoaded = false;

            await Task.Run(() => LoadData(data[0], data[1]));

            IsDataLoaded = true;
        }

        private void LoadData(object selectedExpansionData, object expansionsData)
        {
            SelectedExpansionViewEntity = new SelectedExpansionViewEntity((DominionExpansion)selectedExpansionData);
            ManageCardArtifactsScreenTitle = string.Join(" ", "Manage Artifacts of Expansion", SelectedExpansionViewEntity.ExpansionName);

            InitializeNewArtifact();

            IEnumerable<DominionExpansion> expansions = expansionsData as IEnumerable<DominionExpansion>;
            IEnumerable<AdditionalCard> additionalCards = expansions
                .SelectMany(expansion => expansion.ContainedArtifacts)
                .Where(artifact => artifact.AdditionalCard != null)
                .Select(artifact => artifact.AdditionalCard);
            AdditionalCards.AddRange(additionalCards);
        }

        internal void AddArtifact()
        {
            Check.RequireNotNull(SelectedExpansionViewEntity, nameof(SelectedExpansionViewEntity), nameof(ManageCardArtifactViewModel));
            Check.RequireNotNull(SelectedExpansionViewEntity.ContainedArtifacts, nameof(SelectedExpansionViewEntity.ContainedArtifacts), nameof(ManageCardArtifactViewModel));

            CheckForAdditionalCardType();

            InsertArtifact();
        }

        private void CheckForAdditionalCardType()
        {
            if (SelectedAdditionalCardType == TypeOfAdditionalCard.None)
            {
                NewArtifact.AdditionalCard = null;
                NewArtifact.AdditionalCardId = null;
            }
            else
            {
                HandleArtifactWithAdditionalCard();
            }
        }

        private void HandleArtifactWithAdditionalCard()
        {
            NewArtifact.AdditionalCard.AlreadyIncludedCard = SelectedAdditionalCardType == TypeOfAdditionalCard.Existing;
            AdditionalCard matchedAdditionalCard = AdditionalCards.SingleOrDefault(card => card.Equals(NewArtifact.AdditionalCard));

            if (matchedAdditionalCard == null)
            {
                AdditionalCards.Add(NewArtifact.AdditionalCard);
            }
            else
            {
                NewArtifact.AdditionalCard = matchedAdditionalCard;
            }
        }

        private void InsertArtifact()
        {
            CardArtifactBc.InsertArtifact(NewArtifact);

            SelectedExpansionViewEntity.ContainedArtifacts.Add(NewArtifact);

            InitializeNewArtifact();
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            AdditionalCards.Clear();
            SelectedExpansionViewEntity = null;
            ManageCardArtifactsScreenTitle = null;
            NewArtifact = null;
            SelectedAdditionalCardType = TypeOfAdditionalCard.None;
        }

        private void InitializeNewArtifact()
        {
            NewArtifact = new CardArtifact
            {
                AdditionalCard = new AdditionalCard()
            };
            NewArtifact.AdditionalCardId = NewArtifact.AdditionalCard.Id;
            NewArtifact.DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id;
        }

        #endregion Methods
    }
}
