using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using DSG.Common;
using System.Collections.Generic;
using DSG.BusinessComponents.Cards;
using System.Windows.Input;
using System.Linq;
using DSG.Presentation.ViewEntity;
using DSG.BusinessEntities.GetEnums;
using DSG.BusinessEntities.CardTypes;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardArtifacts;

namespace DSG.Presentation.ViewModel.Management
{
    public class ManageCardsViewModel : AbstractViewModel
    {
        #region Fields

        private ICardBc _cardBc;
        private SelectedExpansionViewEntity _selectedExpansionViewEntity;
        private string _manageCardsScreenTitle;
        private List<IsCardTypeSelectedDto> _selectedCardTypes;
        private List<IsCardSubTypeSelectedDto> _selectedCardSubTypes;
        private List<IsCardArtifactSelectedDto> _selectedCardArtifacts;
        private Card _newCard;

        #endregion Fields


        #region Injected Dependencies

        public ICardBc CardBc
        {
            get
            {
                Check.RequireInjected(_cardBc, nameof(CardBc), nameof(ManageCardsViewModel));
                return _cardBc;
            }
            set { _cardBc = value; }
        }

        #endregion Injected Dependencies


        #region Properties

        public List<Cost> AvailableCosts { get; set; }

        public Card NewCard
        {
            get { return _newCard; }
            set
            {
                _newCard = value;
                OnPropertyChanged(nameof(NewCard));
            }
        }

        public string ManageCardsScreenTitle
        {
            get { return _manageCardsScreenTitle ?? "Manage Cards of Expansion ???"; }
            set
            {
                _manageCardsScreenTitle = value;
                OnPropertyChanged(nameof(ManageCardsScreenTitle));
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

        public List<IsCardTypeSelectedDto> SelectedCardTypes
        {
            get { return _selectedCardTypes; }
            set
            {
                _selectedCardTypes = value;
                OnPropertyChanged(nameof(SelectedCardTypes));
            }
        }

        public List<IsCardSubTypeSelectedDto> SelectedCardSubTypes
        {
            get { return _selectedCardSubTypes; }
            set
            {
                _selectedCardSubTypes = value;
                OnPropertyChanged(nameof(SelectedCardSubTypes));
            }
        }

        public List<IsCardArtifactSelectedDto> SelectedCardArtifacts
        {
            get { return _selectedCardArtifacts; }
            set
            {
                _selectedCardArtifacts = value;
                OnPropertyChanged(nameof(SelectedCardArtifacts));
            }
        }

        #endregion Properties


        public ManageCardsViewModel()
        {
            AvailableCosts = new List<Cost>();
            AddCardCommand = new RelayCommand(cmd => AddCard());
            NavigateToManageSetsScreenCommand = new RelayCommand(async cmd => await NavigateToAsync(NavigationDestination.ManageSets));
        }

        #region Commands

        public ICommand AddCardCommand { get; set; }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }

        #endregion Commands


        #region Methods

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            IsDataLoaded = false;
            Task dataLoadingTask = Task.Run(() => LoadData(data[0], data[1]));
            await dataLoadingTask;
            IsDataLoaded = true;
        }

        private void LoadData(object selectedExpansionData, object expansionData)
        {
            SelectedExpansionViewEntity = new SelectedExpansionViewEntity((DominionExpansion)selectedExpansionData);

            ManageCardsScreenTitle = string.Join(" ", "Manage Cards of Expansion", SelectedExpansionViewEntity.ExpansionName);

            IEnumerable<DominionExpansion> expansions = expansionData as IEnumerable<DominionExpansion>;
            AvailableCosts.AddRange(
                expansions
                .SelectMany(expansion => expansion.ContainedCards)
                .Select(card => card.Cost)
                .Distinct()
                .ToList());

            LoadEnums();
            InitializeNewCard();

            SelectedCardArtifacts = SelectedExpansionViewEntity
                .ContainedArtifacts
                .Select(artifact => new IsCardArtifactSelectedDto { ArtifactName = artifact.Name, IsSelected = false, Artifact = artifact })
                .ToList();
        }

        private void LoadEnums()
        {
            List<CardTypeEnum> cardTypes = GetEnum.CardType();
            SelectedCardTypes = cardTypes.Select(cardType => new IsCardTypeSelectedDto { CardType = cardType, IsSelected = false }).ToList();

            List<CardSubTypeEnum> cardSubTypes = GetEnum.CardSubType();
            SelectedCardSubTypes = cardSubTypes.Select(subType => new IsCardSubTypeSelectedDto { CardSubType = subType, IsSelected = false }).ToList();
        }

        public void AddCard()
        {
            Check.RequireNotNull(SelectedExpansionViewEntity, nameof(SelectedExpansionViewEntity), nameof(ManageCardsViewModel));
            Check.RequireNotNullNotEmpty(NewCard.Name, nameof(NewCard.Name), nameof(ManageCardsViewModel));

            CheckForMatchingCost();
            InsertCard();
            ResetData();
        }

        private void CheckForMatchingCost()
        {
            Cost matchingCost = AvailableCosts.SingleOrDefault(cost => cost.Equals(NewCard.Cost));

            if (matchingCost == null)
            {
                AvailableCosts.Add(NewCard.Cost);
            }
            else
            {
                NewCard.Cost = matchingCost;
            }
        }

        private void ResetData()
        {
            InitializeNewCard();
            SelectedCardArtifacts.ForEach(artifact => artifact.IsSelected = false);
            SelectedCardSubTypes.ForEach(type => type.IsSelected = false);
        }

        private void InsertCard()
        {
            NewCard.CardTypeToCards = SelectedCardTypes
                .Where(type => type.IsSelected == true)
                .Select(type => type.CardType)
                .Select(type => new CardTypeToCard { CardTypeId = type })
                .ToList();

            NewCard.CardSubTypeToCards = SelectedCardSubTypes
                .Where(type => type.IsSelected == true)
                .Select(type => type.CardSubType)
                .Select(type => new CardSubTypeToCard { CardSubTypeId = type })
                .ToList();

            NewCard.CardArtifactsToCard = SelectedCardArtifacts
                .Where(artifact => artifact.IsSelected == true)
                .Select(artifact => artifact.Artifact)
                .Select(artifact => new CardArtifactToCard { CardArtifact = artifact })
                .ToList();

            CardBc.InsertCard(NewCard);
            SelectedExpansionViewEntity.ContainedCards.Add(NewCard);
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            SelectedExpansionViewEntity = null;
            ManageCardsScreenTitle = null;
            SelectedCardTypes = null;
            SelectedCardArtifacts = null;
            SelectedCardSubTypes = null;
            AvailableCosts.Clear();
            NewCard = null;
        }

        private void InitializeNewCard()
        {
            NewCard = new Card
            {
                Cost = NewCard?.Cost == null ? new Cost() : NewCard.Cost.Clone()
            };
            NewCard.CostId = NewCard.Cost.Id;
            NewCard.DominionExpansion = SelectedExpansionViewEntity.DominionExpansion;
            NewCard.DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id;
        }

        #endregion Methods
    }
}
