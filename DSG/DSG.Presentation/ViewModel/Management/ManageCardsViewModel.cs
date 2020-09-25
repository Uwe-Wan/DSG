﻿using DSG.BusinessEntities;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using DSG.Common;
using System.Collections.Generic;
using DSG.BusinessComponents.Cards;
using System.Windows.Input;
using System.Linq;
using DSG.Presentation.ViewEntity;
using System;
using DSG.BusinessEntities.GetEnums;
using DSG.BusinessEntities.CardTypes;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardArtifacts;

namespace DSG.Presentation.ViewModel.Management
{
    public class ManageCardsViewModel : AbstractViewModel
    {
        private Card _selectedCard;
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

        public ICardBc CardBc
        {
            get
            {
                Check.RequireInjected(_cardBc, nameof(CardBc), nameof(ManageCardsViewModel));
                return _cardBc;
            }
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

        public List<IsCardTypeSelectedDto> SelectedCardTypes { get; set; }

        public List<IsCardSubTypeSelectedDto> SelectedCardSubTypes { get; set; }

        public List<IsCardArtifactSelectedDto> SelectedCardArtifacts { get; set; }

        public bool MenuOpened { get; set; }

        public ManageCardsViewModel()
        {
            AvailableCosts = new List<Cost>();
            AddCardCommand = new RelayCommand(cmd => AddCard());
            NavigateToManageSetsScreenCommand = new RelayCommand(cmd => NavigateTo(NavigationDestination.ManageSets));
            MenuCommand = new RelayCommand(cmd => OpenDropdownMenu());
            MenuOpened = false;
        }

        public ICommand AddCardCommand { get; set; }

        public ICommand NavigateToManageSetsScreenCommand { get; private set; }

        public ICommand MenuCommand { get; private set; }

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            SelectedExpansionViewEntity = new SelectedExpansionViewEntity((DominionExpansion)data[0]);

            IEnumerable<DominionExpansion> expansions = data[1] as IEnumerable<DominionExpansion>;
            AvailableCosts.Clear();
            AvailableCosts.AddRange(
                expansions
                .SelectMany(expansion => expansion.ContainedCards)
                .Select(card => card.Cost)
                .Distinct()
                .ToList());

            ManageCardsScreenTitle = string.Join(" ", "Manage Cards of Expansion", SelectedExpansionViewEntity.ExpansionName);

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

        private void OpenDropdownMenu()
        {
            MenuOpened = !MenuOpened;
        }
    }
}