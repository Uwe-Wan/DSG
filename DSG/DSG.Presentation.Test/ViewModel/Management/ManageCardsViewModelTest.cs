﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSG.BusinessComponents.Cards;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using DSG.Presentation.ViewModel.Management;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DSG.Presentation.Test.ViewModel.Management
{
    [TestFixture]
    public class ManageCardsViewModelTest
    {
        private ManageCardsViewModel _testee;
        private Mock<ICardBc> _cardBcMock;
        private Mock<INaviService> _naviMock;

        [SetUp]
        public void Setup()
        {
            _testee = new ManageCardsViewModel();

            _cardBcMock = new Mock<ICardBc>();
            _testee.CardBc = _cardBcMock.Object;

            _naviMock = new Mock<INaviService>();
            _testee.NaviService = _naviMock.Object;
        }

        [Test]
        public async Task OnPageLoadedAsync_OneAvailableCostInThreeSets_OneSelected()
        {
            //Arrange
            Card chapel = new Card { Name = "Chapel", Cost = new Cost(2, 0, false) };
            List<Card> cards = new List<Card> { chapel };

            DominionExpansion world = new DominionExpansion { Name = "World", ContainedCards = cards };
            DominionExpansion seaside = new DominionExpansion { Name = "Seaside", ContainedCards = new List<Card>() };
            DominionExpansion intrigue = new DominionExpansion { Name = "Intrigue", ContainedCards = new List<Card>() };

            List<DominionExpansion> expansions = new List<DominionExpansion> { world, seaside, intrigue };

            //Act
            await _testee.OnPageLoadedAsync(world, expansions) ;

            //Assert
            _testee.SelectedExpansionViewEntity.DominionExpansion.Should().Be(world);
            _testee.SelectedExpansionViewEntity.ExpansionName.Should().Be(world.Name);
            _testee.SelectedExpansionViewEntity.ContainedCards.Should().HaveCount(1);

            _testee.ManageCardsScreenTitle.Should().Be("Manage Cards of Expansion World");

            _testee.AvailableCosts.Should().HaveCount(1);
            _testee.AvailableCosts.First().Money.Should().Be(2);

            _testee.NewCard.DominionExpansion.Should().BeEquivalentTo(world);
            _testee.NewCard.DominionExpansionId.Should().Be(world.Id);
            _testee.NewCard.Cost.Should().BeEquivalentTo(new Cost());

            CheckCardTypeList();
        }

        private void CheckCardTypeList()
        {
            _testee.SelectedCardTypes.Select(type => type.IsSelected).Should().NotContain(true);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Action);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Victory);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Treasure);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Night);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Event);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Landmark);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Project);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().Contain(CardTypeEnum.Way);
            _testee.SelectedCardTypes.Select(type => type.CardType).Should().HaveCount(8);
        }

        [Test]
        public void AddCard_CostAlreadyAvailable_NotInserted()
        {
            //Arrange
            _testee.AvailableCosts = new List<Cost> { new Cost(money: 2) };
            _testee.NewCard = new Card
            {
                Cost = new Cost(money: 2),
                Name = "Test"
            };

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});
            
            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();
            _testee.SelectedCardSubTypes = new List<IsCardSubTypeSelectedDto>();
            _testee.SelectedCardArtifacts = new List<IsCardArtifactSelectedDto>();

            //Act
            _testee.AddCard();

            //Assert
            _cardBcMock.Verify(bc => bc.InsertCard(It.Is<Card>(card => card.Cost.Money == 2 && card.Cost.Dept == 0 && card.Cost.Potion == false)), Times.Once);
            _cardBcMock.VerifyNoOtherCalls();
            _testee.AvailableCosts.Should().HaveCount(1);
        }

        [Test]
        public void AddCard_CostNotAvailable_Inserted()
        {
            //Arrange
            _testee.AvailableCosts = new List<Cost> { new Cost(2) };
            _testee.NewCard = new Card
            {
                Cost = new Cost(money: 3),
                Name = "Test"
            };

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});

            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();
            _testee.SelectedCardSubTypes = new List<IsCardSubTypeSelectedDto>();
            _testee.SelectedCardArtifacts = new List<IsCardArtifactSelectedDto>();

            //Act
            _testee.AddCard();

            //Assert
            _cardBcMock.Verify(bc => bc.InsertCard(It.Is<Card>(card => card.Cost.Money == 3 && card.Cost.Dept == 0 && card.Cost.Potion == false)), Times.Once);
            _cardBcMock.VerifyNoOtherCalls();
            _testee.AvailableCosts.Should().HaveCount(2);
        }

        [Test]
        public void AddCard_DataReset()
        {
            //Arrange
            _testee.NewCard = new Card
            {
                Cost = new Cost(money: 3),
                Name = "Test"
            };
            _testee.AvailableCosts = new List<Cost>();
            DominionExpansion selectedExpansion = new DominionExpansion { ContainedCards = new List<Card>() };
            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(selectedExpansion);

            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto> { new IsCardTypeSelectedDto { IsSelected = true } };
            _testee.SelectedCardSubTypes = new List<IsCardSubTypeSelectedDto> { new IsCardSubTypeSelectedDto { IsSelected = true } };
            _testee.SelectedCardArtifacts = new List<IsCardArtifactSelectedDto> { new IsCardArtifactSelectedDto { IsSelected = true } };

            //Act
            _testee.AddCard();

            //Assert
            _testee.SelectedCardArtifacts.First().IsSelected.Should().BeFalse();
            _testee.SelectedCardTypes.First().IsSelected.Should().BeTrue();
            _testee.SelectedCardSubTypes.First().IsSelected.Should().BeFalse();

            _testee.NewCard.Cost.Money.Should().Be(3);
            _testee.NewCard.DominionExpansion.Should().BeEquivalentTo(selectedExpansion);
            _testee.NewCard.DominionExpansionId.Should().Be(selectedExpansion.Id);
            _testee.NewCard.Name.Should().BeNull();
        }

        [Test]
        public async Task NavigateToAsync_NavigationInvoked_DataCleared()
        {
            //Arrange
            _testee.NewCard = new Card
            {
                Cost = new Cost(money: 2, dept: 3, potion: true),
                Name = "Test"
            };
            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion());
            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();
            _testee.SelectedCardArtifacts = new List<IsCardArtifactSelectedDto>();
            _testee.SelectedCardSubTypes = new List<IsCardSubTypeSelectedDto>();
            _testee.ManageCardsScreenTitle = "Title";

            //Act
            await _testee.NavigateToAsync(NavigationDestination.ManageCardArtifacts);

            //Assert
            _naviMock.Verify(x => x.NavigateToAsync(NavigationDestination.ManageCardArtifacts), Times.Once);

            _testee.NewCard.Should().BeNull();
            _testee.SelectedExpansionViewEntity.Should().BeNull();
            _testee.SelectedCardTypes.Should().BeNull();
            _testee.SelectedCardArtifacts.Should().BeNull();
            _testee.SelectedCardSubTypes.Should().BeNull();
            _testee.ManageCardsScreenTitle.Should().Be("Manage Cards of Expansion ???");
        }
    }
}
