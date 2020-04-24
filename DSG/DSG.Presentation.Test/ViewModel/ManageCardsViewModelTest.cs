using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSG.BusinessComponents.Cards;
using DSG.BusinessComponents.Costs;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using DSG.Presentation.ViewModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class ManageCardsViewModelTest
    {
        private ManageCardsViewModel _testee;
        private Mock<ICardBc> _cardBcMock;
        private Mock<ICostBc> _costBcMock;

        [SetUp]
        public void Setup()
        {
            _testee = new ManageCardsViewModel();

            _cardBcMock = new Mock<ICardBc>();
            _testee.CardBc = _cardBcMock.Object;

            _costBcMock = new Mock<ICostBc>();
            _testee.CostBc = _costBcMock.Object;
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
            await _testee.OnPageLoadedAsync(NavigationDestination.ManageCards, world, expansions) ;

            //Assert
            _testee.SelectedExpansionViewEntity.DominionExpansion.Should().Be(world);
            _testee.SelectedExpansionViewEntity.ExpansionName.Should().Be(world.Name);
            _testee.SelectedExpansionViewEntity.ContainedCards.Should().HaveCount(1);

            _testee.ManageCardsScreenTitle.Should().Be("Manage Cards of Expansion World");

            _testee.AvailableCosts.Should().HaveCount(1);
            _testee.AvailableCosts.First().Money.Should().Be(2);
        }

        [Test]
        public void AddCard_CostAlreadyAvailable_NotInserted()
        {
            //Arrange
            _testee.AvailableCosts = new List<Cost> { new Cost(2) };
            _testee.NewCardCostsPotion = false;
            _testee.NewCardsCost = 2;
            _testee.NewCardsDept = 0;

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});

            //Act
            _testee.AddCard();

            //Assert
            _costBcMock.Verify(bc => bc.InsertCost(It.IsAny<Cost>()), Times.Never);
            _cardBcMock.Verify(bc => bc.InsertCard(It.Is<Card>(card => card.Cost.Money == 2 && card.Cost.Dept == 0 && card.Cost.Potion == false)), Times.Once);
            _cardBcMock.VerifyNoOtherCalls();
        }

        [Test]
        public void AddCard_CostNotAvailable_Inserted()
        {
            //Arrange
            _testee.AvailableCosts = new List<Cost> { new Cost(2) };
            _testee.NewCardCostsPotion = false;
            _testee.NewCardsCost = 3;
            _testee.NewCardsDept = 0;

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});

            //Act
            _testee.AddCard();

            //Assert
            _costBcMock.Verify(bc => bc.InsertCost(It.Is<Cost>(cost => cost.Money == 3)), Times.Once);
            _costBcMock.VerifyNoOtherCalls();
            _cardBcMock.Verify(bc => bc.InsertCard(It.Is<Card>(card => card.Cost.Money == 3 && card.Cost.Dept == 0 && card.Cost.Potion == false)), Times.Once);
            _cardBcMock.VerifyNoOtherCalls();
        }
    }
}
