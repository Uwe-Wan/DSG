﻿using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using DSG.Common;
using System.Collections.Generic;
using DSG.BusinessComponents.Costs;
using DSG.BusinessComponents.Cards;
using System.Windows.Input;
using System.Linq;
using DSG.Presentation.ViewEntity;
using System;
using DSG.BusinessComponents;

namespace DSG.Presentation.ViewModel
{
    public class ManageCardsViewModel : Notifier, IViewModel
    {
        private Card _selectedCard;
        private ICostBc _costBc;
        private ICardBc _cardBc;
        private string _newCardsName;
        private int? _newCardsCost;
        private int? _newCardsDept;
        private bool _newCardCostsPotion;
        private LookupBc<CardType> _lookupBc;

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

        public LookupBc<CardType> LookupBc
        {
            get { return _lookupBc ?? new LookupBc<CardType>(); }
            set { _lookupBc = value; }
        }

        public string ManageCardsScreenTitle { get; set; }

        public SelectedExpansionViewEntity SelectedExpansionViewEntity { get; set; }

        public List<IsCardTypeSelectedDto> SelectedCardTypes { get; set; }

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
            AvailableCosts.Clear();
            AvailableCosts.AddRange(
                enumData
                .SelectMany(expansion => expansion.ContainedCards)
                .Select(card => card.Cost)
                .Distinct()
                .ToList());

            ManageCardsScreenTitle = string.Join(" ", "Manage Cards of Expansion", SelectedExpansionViewEntity.ExpansionName);

            IEnumerable<CardTypeEnum> enums = Enum.GetValues(typeof(CardTypeEnum)) as IEnumerable<CardTypeEnum>;
            SelectedCardTypes = enums.Select(cardType => new IsCardTypeSelectedDto { CardType = cardType, IsSelected = false }).ToList();
        }

        public void AddCard()
        {
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
            List<CardType> cardTypes = LookupBc.GetLookup();

            List<CardTypeToCard> cardTypeToCards = SelectedCardTypes
                .Where(type => type.IsSelected == true)
                .Select(type => type.CardType)
                .Select(type => new CardTypeToCard { CardTypeId = type })
                .ToList();

            Card card = new Card
            {
                Cost = cost,
                Name = NewCardsName,
                DominionExpansion = SelectedExpansionViewEntity.DominionExpansion,
                CostId = cost.Id,
                DominionExpansionId = SelectedExpansionViewEntity.DominionExpansion.Id,
                CardTypeToCards = cardTypeToCards
                //CardTypeToCards = cardTypes.Where(types => types.Id == CardTypeEnum.Action).Select(type => new CardTypeToCard {CardTypeId = type.Id }).ToList()
            };
            CardBc.InsertCard(card);

            SelectedExpansionViewEntity.ContainedCards.Add(card);
        }
    }
}
