using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using DSG.Common;
using System.Collections.Generic;
using DSG.BusinessComponents.Costs;
using DSG.BusinessComponents.Cards;
using System.Windows.Input;
using System.Linq;
using DSG.Presentation.ViewEntity;

namespace DSG.Presentation.ViewModel
{
    public class ManageCardsViewModel : Notifier, IViewModel
    {
        private Card _selectedCard;
        private DominionExpansion _selectedExpansion;
        private ICostBc _costBc;
        private ICardBc _cardBc;
        private string _newCardsName;
        private int? _newCardsCost;
        private int? _newCardsDept;
        private bool _newCardCostsPotion;

        public Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public ICostBc CostBc
        {
            get { return _costBc; }
            set { _costBc = value; }
        }

        public ICardBc CardBc
        {
            get { return _cardBc; }
            set { _cardBc = value; }
        }

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

        public string ManageCardsScreenTitle { get; set; }

        public SelectedExpansionViewEntity SelectedExpansionViewEntity { get; set; }

        public ManageCardsViewModel()
        {
            AvailableCosts = new List<Cost>();
            AddCardCommand = new RelayCommand(cmd => AddCard());
        }

        public ICommand AddCardCommand { get; set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            SelectedExpansionViewEntity = new SelectedExpansionViewEntity((DominionExpansion)data[0]);

            IEnumerable<DominionExpansion> enumData = data[1] as IEnumerable<DominionExpansion>;
            foreach (DominionExpansion expansion in enumData)
            {
                AvailableCosts.AddRange(expansion
                    .ContainedCards
                    .Select(card => card.Cost)
                    .ToList()
                    );
            }

            ManageCardsScreenTitle = string.Join(" ", "Manage Cards of Expansion", SelectedExpansionViewEntity.ExpansionName);
        }

        public void AddCard()
        {
            Cost newCost = new Cost(NewCardsCost ?? 0, NewCardsDept ?? 0, NewCardCostsPotion);
            Cost matchingCost = AvailableCosts.SingleOrDefault(cost => cost.Equals(newCost));

            if (matchingCost == null)
            {
                CostBc.InsertCost(newCost);
                InsertCard(newCost);
            }
            else
            {
                InsertCard(matchingCost);
            }
        }

        private void InsertCard(Cost cost)
        {
            Card card = new Card
            {
                Cost = cost,
                Name = NewCardsName,
                DominionExpansion = SelectedExpansionViewEntity.DominionExpansion,
                CostId = cost.Id,
                DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id
            };
            CardBc.InsertCard(card);

            SelectedExpansionViewEntity.ContainedCards.Add(card);
        }
    }
}
