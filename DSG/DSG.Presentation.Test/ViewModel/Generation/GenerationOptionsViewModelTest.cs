using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
using DSG.Presentation.Services;
using DSG.Presentation.ViewModel.Generation;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel.Generation
{
    [TestFixture]
    public class GenerationOptionsViewModelTest
    {
        private GenerationOptionsViewModel _testee;
        private Mock<IUiService> _uiServiceMock;
        private Mock<INaviService> _naviServiceMock;

        [Test]
        public async Task OnPageLoaded_NoParameterSet_ParameterSet()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeTrue();
            _testee.GenerationParameter.PropabilityForColonyAndPlatinum.Should().Be(20);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount.Should().HaveCount(4);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[1].Should().Be(50);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[2].Should().Be(30);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[3].Should().Be(7);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[4].Should().Be(0);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSet_NothingDone()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 42);
            propabilitiesForNonSuppliesByAmount.Add(2, 14);
            propabilitiesForNonSuppliesByAmount.Add(3, 2);

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) };
            isDominionExpansionSelectedDtos.First().IsSelected = false;

            _testee.GenerationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 30, propabilitiesForNonSuppliesByAmount);

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeFalse();
            _testee.GenerationParameter.PropabilityForColonyAndPlatinum.Should().Be(30);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount.Should().HaveCount(3);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[1].Should().Be(42);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[2].Should().Be(14);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[3].Should().Be(2);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSetWithDifferentAmountOfExpansions_ExpansionsRefreshed()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 42);
            propabilitiesForNonSuppliesByAmount.Add(2, 14);
            propabilitiesForNonSuppliesByAmount.Add(3, 2);

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion, new DominionExpansion() };
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) };
            isDominionExpansionSelectedDtos.First().IsSelected = false;

            _testee.GenerationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 30, propabilitiesForNonSuppliesByAmount);

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.Should().HaveCount(2);
            _testee.GenerationParameter.IsDominionExpansionSelectedDtos.First().IsSelected.Should().BeTrue();
            _testee.GenerationParameter.PropabilityForColonyAndPlatinum.Should().Be(30);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount.Should().HaveCount(3);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[1].Should().Be(42);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[2].Should().Be(14);
            _testee.GenerationParameter.PropabilitiesForNonSuppliesByAmount[3].Should().Be(2);
        }

        [Test]
        public void GenerateSet_NotEnoughCards_OpenUiWindow()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            _uiServiceMock = new Mock<IUiService>();
            _testee.UiService = _uiServiceMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            CardTypeToCard supplyType = new CardTypeToCard { CardType = new CardType { IsSupplyType = true } };

            DominionExpansion expansion = new DominionExpansion
            {
                ContainedCards = new List<Card>
                {
                    new Card { CardTypeToCards = new List<CardTypeToCard> { supplyType } }
                }
            };

            _testee.GenerationParameter = new GenerationParameterDto(
                new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) },
                20,
                new Dictionary<int, int>());

            //Act
            _testee.GenerateSet();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage("There are only 1 cards available in the selected Sets. " +
                    "A minimum of 10 is needed to generate a set.", "Not enough Cards!"), Times.Once);
            _naviServiceMock.Verify(x => x.NavigateToAsync(NavigationDestination.GeneratedSet, It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void GenerateSet_EnoughCards_NavigationInvoked()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            _uiServiceMock = new Mock<IUiService>();
            _testee.UiService = _uiServiceMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            DominionExpansion expansion = new DominionExpansion { ContainedCards = new List<Card>() };
            CardTypeToCard supplyType = new CardTypeToCard { CardType = new CardType { IsSupplyType = true } };

            for (int i = 0; i < 15; i++)
            {
                expansion.ContainedCards.Add(
                    new Card { CardTypeToCards = new List<CardTypeToCard> { supplyType } });
            }

            _testee.GenerationParameter = new GenerationParameterDto(
                new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) },
                20,
                new Dictionary<int, int> { { 1, 20 } });

            //Act
            _testee.GenerateSet();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _naviServiceMock.Verify(x => x.NavigateToAsync(                
                    NavigationDestination.GeneratedSet,
                    It.Is<GenerationParameterDto>(parameter => 
                        parameter.PropabilityForColonyAndPlatinum == 20 && 
                        parameter.Expansions.First().Equals(expansion) &&
                        parameter.PropabilitiesForNonSuppliesByAmount.First().Value == 20)),
                Times.Once);
        }
    }
}
