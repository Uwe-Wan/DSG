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
        private string _newCardsName;
        private int? _newCardsCost;
        private int? _newCardsDept;
        private bool _newCardCostsPotion;
        private SelectedExpansionViewEntity _selectedExpansionViewEntity;
        private string _manageCardsScreenTitle;
        private List<IsCardTypeSelectedDto> _selectedCardTypes;
        private List<IsCardSubTypeSelectedDto> _selectedCardSubTypes;
        private List<IsCardArtifactSelectedDto> _selectedCardArtifacts;

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

        public string NewCardsName
        {
            get { return _newCardsName; }
            set
            {
                _newCardsName = value;
                OnPropertyChanged(nameof(NewCardsName));
            }
        }

        public int? NewCardsCost
        {
            get { return _newCardsCost; }
            set
            {
                _newCardsCost = value;
                OnPropertyChanged(nameof(NewCardsCost));
            }
        }

        public int? NewCardsDept
        {
            get { return _newCardsDept; }
            set
            {
                _newCardsDept = value;
                OnPropertyChanged(nameof(NewCardsDept));
            }
        }

        public bool NewCardCostsPotion
        {
            get { return _newCardCostsPotion; }
            set
            {
                _newCardCostsPotion = value;
                OnPropertyChanged(nameof(NewCardCostsPotion));
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

            List<CardTypeEnum> cardTypes = GetEnum.CardType();
            SelectedCardTypes = cardTypes.Select(cardType => new IsCardTypeSelectedDto { CardType = cardType, IsSelected = false }).ToList();

            List<CardSubTypeEnum> cardSubTypes = GetEnum.CardSubType();
            SelectedCardSubTypes = cardSubTypes.Select(subType => new IsCardSubTypeSelectedDto { CardSubType = subType, IsSelected = false }).ToList();

            SelectedCardArtifacts = SelectedExpansionViewEntity
                .ContainedArtifacts
                .Select(artifact => new IsCardArtifactSelectedDto { ArtifactName = artifact.Name, IsSelected = false, Artifact = artifact })
                .ToList();
        }

        public void AddCard()
        {
            Check.RequireNotNull(SelectedExpansionViewEntity, nameof(SelectedExpansionViewEntity), nameof(ManageCardsViewModel));
            Check.RequireNotNullNotEmpty(NewCardsName, nameof(NewCardsName), nameof(ManageCardsViewModel));

            Cost newCost = new Cost(NewCardsCost ?? 0, NewCardsDept ?? 0, NewCardCostsPotion);
            Cost matchingCost = AvailableCosts.SingleOrDefault(cost => cost.Equals(newCost));

            if (matchingCost == null)
            {
                AvailableCosts.Add(newCost);
                InsertCard(newCost);
            }
            else
            {
                InsertCard(matchingCost);
            }
        }

        private void InsertCard(Cost cost)
        {
            List<CardTypeToCard> cardTypeToCards = SelectedCardTypes
                .Where(type => type.IsSelected == true)
                .Select(type => type.CardType)
                .Select(type => new CardTypeToCard { CardTypeId = type })
                .ToList();

            List<CardSubTypeToCard> cardSubTypeToCards = SelectedCardSubTypes
                .Where(type => type.IsSelected == true)
                .Select(type => type.CardSubType)
                .Select(type => new CardSubTypeToCard { CardSubTypeId = type })
                .ToList();

            List<CardArtifactToCard> cardArtifactToCards = SelectedCardArtifacts
                .Where(artifact => artifact.IsSelected == true)
                .Select(artifact => artifact.Artifact)
                .Select(artifact => new CardArtifactToCard { CardArtifact = artifact })
                .ToList();

            Card card = new Card
            {
                Cost = cost,
                Name = NewCardsName,
                DominionExpansion = SelectedExpansionViewEntity.DominionExpansion,
                CostId = cost.Id,
                DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id,
                CardTypeToCards = cardTypeToCards,
                CardSubTypeToCards = cardSubTypeToCards,
                CardArtifactsToCard = cardArtifactToCards
            };
            CardBc.InsertCard(card);

            SelectedExpansionViewEntity.ContainedCards.Add(card);
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
            NewCardCostsPotion = false;
            NewCardsCost = null;
            NewCardsDept = null;
            NewCardsName = null;
        }

        #endregion Methods
    }
}
