using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSG.BusinessComponents.Cards;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
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

        [SetUp]
        public void Setup()
        {
            _testee = new ManageCardsViewModel();

            _cardBcMock = new Mock<ICardBc>();
            _testee.CardBc = _cardBcMock.Object;
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
            _testee.AvailableCosts = new List<Cost> { new Cost(2) };
            _testee.NewCardCostsPotion = false;
            _testee.NewCardsCost = 2;
            _testee.NewCardsDept = 0;

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});
            
            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();

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
            _testee.NewCardCostsPotion = false;
            _testee.NewCardsCost = 3;
            _testee.NewCardsDept = 0;
            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { ContainedCards = new List<Card>()});

            _testee.SelectedCardTypes = new List<IsCardTypeSelectedDto>();

            //Act
            _testee.AddCard();

            //Assert
            _cardBcMock.Verify(bc => bc.InsertCard(It.Is<Card>(card => card.Cost.Money == 3 && card.Cost.Dept == 0 && card.Cost.Potion == false)), Times.Once);
            _cardBcMock.VerifyNoOtherCalls();
            _testee.AvailableCosts.Should().HaveCount(2);
        }
    }
}
