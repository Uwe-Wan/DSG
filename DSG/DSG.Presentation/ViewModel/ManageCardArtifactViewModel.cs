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

namespace DSG.Presentation.ViewModel
{
    public class ManageCardArtifactViewModel : AbstractViewModel
    {
        private ICardArtifactBc _cardArtifactBc;
        private AdditionalCard _selectedAdditionalCard;
        private int? _selectedAmountOfAdditionalCards;
        public string _nameOfNewArtifact;

        public ICardArtifactBc CardArtifactBc
        {
            get
            {
                Check.RequireInjected(_cardArtifactBc, nameof(CardArtifactBc), nameof(ManageCardArtifactViewModel));
                return _cardArtifactBc;
            }
            set { _cardArtifactBc = value; }
        }

        public string ManageCardArtifactsScreenTitle { get; set; }

        public SelectedExpansionViewEntity SelectedExpansionViewEntity { get; set; }

        public ObservableCollection<AdditionalCard> AdditionalCards { get; set; }

        public List<TypeOfAdditionalCard> AvailableTypesOfAdditionalCard { get; set; }

        public TypeOfAdditionalCard SelectedAdditionalCardType { get; set; }

        public int? MaxCost { get; set; }

        public int? MinCost { get; set; }

        public AdditionalCard SelectedAdditionalCard
        {
            get { return _selectedAdditionalCard; }
            set
            {
                _selectedAdditionalCard = value;
                OnPropertyChanged(nameof(SelectedAdditionalCard));
            }
        }

        public int? SelectedAmountOfAdditionalCards
        {
            get { return _selectedAmountOfAdditionalCards; }
            set
            {
                _selectedAmountOfAdditionalCards = value;
                OnPropertyChanged(nameof(SelectedAmountOfAdditionalCards));
            }
        }

        public string NameOfNewArtifact
        {
            get { return _nameOfNewArtifact; }
            set
            {
                _nameOfNewArtifact = value;
                OnPropertyChanged(nameof(NameOfNewArtifact));
            }
        }

        public ICommand AddArtifactCommand { get; private set; }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }

        public ManageCardArtifactViewModel()
        {
            AddArtifactCommand = new RelayCommand(c => AddArtifact());
            AdditionalCards = new ObservableCollection<AdditionalCard>();
            SelectedAdditionalCard = new AdditionalCard();
            AvailableTypesOfAdditionalCard = Enum.GetValues(typeof(TypeOfAdditionalCard))
                    .OfType<TypeOfAdditionalCard>()
                    .ToList();

            NavigateToManageSetsScreenCommand = new RelayCommand(cmd => NavigateTo(NavigationDestination.ManageSets));
        }

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            AdditionalCards.Clear();

            IEnumerable<DominionExpansion> expansions = data[1] as IEnumerable<DominionExpansion>;
            IEnumerable<AdditionalCard> additionalCards = expansions
                .SelectMany(expansion => expansion.ContainedArtifacts)
                .Where(artifact => artifact.AdditionalCard != null)
                .Select(artifact => artifact.AdditionalCard);
            AdditionalCards.AddRange(additionalCards);

            SelectedExpansionViewEntity = new SelectedExpansionViewEntity((DominionExpansion)data[0]);
            ManageCardArtifactsScreenTitle = string.Join(" ", "Manage Artifacts of Expansion", SelectedExpansionViewEntity.ExpansionName);
        }

        internal void AddArtifact()
        {
            Check.RequireNotNull(SelectedExpansionViewEntity, nameof(SelectedExpansionViewEntity), nameof(ManageCardArtifactViewModel));
            Check.RequireNotNull(SelectedExpansionViewEntity.ContainedArtifacts, nameof(SelectedExpansionViewEntity.ContainedArtifacts), nameof(ManageCardArtifactViewModel));

            if (SelectedAdditionalCardType == TypeOfAdditionalCard.None)
            {
                InsertArtifact(null);
                return;
            }

            HandleArtifactWithAdditionalCard();
        }

        private void HandleArtifactWithAdditionalCard()
        {
            SelectedAdditionalCard.AlreadyIncludedCard = SelectedAdditionalCardType == TypeOfAdditionalCard.Existing;

            AdditionalCard additionalCard = new AdditionalCard(MinCost, MaxCost, SelectedAdditionalCardType);

            AdditionalCard matchedAdditionalCard = AdditionalCards.SingleOrDefault(card => card.Equals(additionalCard));

            if (matchedAdditionalCard == null)
            {
                AdditionalCards.Add(additionalCard);
                InsertArtifact(additionalCard);
            }
            else
            {
                InsertArtifact(matchedAdditionalCard);
            }
        }

        private void InsertArtifact(AdditionalCard additionalCard)
        {
            CardArtifact newArtifact = new CardArtifact
            {
                Name = NameOfNewArtifact,
                AdditionalCard = additionalCard,
                AdditionalCardId = additionalCard?.Id,
                AmountOfAdditionalCards = SelectedAmountOfAdditionalCards.HasValue ? SelectedAmountOfAdditionalCards.Value : 0,
                DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id
            };

            CardArtifactBc.InsertArtifact(newArtifact);

            SelectedExpansionViewEntity.ContainedArtifacts.Add(newArtifact);
        }
    }
}
