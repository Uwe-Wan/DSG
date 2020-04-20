using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using DSG.Common;
using System.Collections.Generic;
using DSG.BusinessComponents.Costs;
using DSG.BusinessComponents.Cards;
using System.Windows.Input;
using System.Linq;

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

        public DominionExpansion SelectedExpansion
        {
            get { return _selectedExpansion; }
            set
            {
                _selectedExpansion = value;
                OnPropertyChanged(nameof(SelectedExpansion));
            }
        }

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

        public ManageCardsViewModel()
        {
            AvailableCosts = new List<Cost>();
        }

        public ICommand AddCardCommand;

        public async Task OnPageLoadedAsync(NavigationDestination navigationDestination, params object[] data)
        {
            SelectedExpansion = (DominionExpansion)data[0];

            IEnumerable<DominionExpansion> enumData = data[1] as IEnumerable<DominionExpansion>;
            foreach (DominionExpansion expansion in enumData)
            {
                AvailableCosts.AddRange(expansion
                    .ContainedCards
                    .Select(card => card.Cost)
                    .ToList()
                    );
            }
        }

        public void AddCard()
        {
            Cost newCost = new Cost { Money = NewCardsCost ?? 0, Dept = NewCardsDept ?? 0, Potion = NewCardCostsPotion };
            if(AvailableCosts.Contains(newCost) == false)
            {
                CostBc.InsertCost(newCost);
            }
        }
    }
}
