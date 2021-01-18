﻿using DSG.BusinessComponents.Generation;
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
        public async Task OnPageLoaded_BcInvokedWithInputExpansions()
        {
            //Arrange
            _testee = new GenerationOptionsViewModel();

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeTrue();
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

            _testee.IsDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) };

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

            _testee.IsDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto> { new IsDominionExpansionSelectedDto(expansion) };
            _testee.PropabilityOfPlatinumAndColony = 20;

            GenerationParameterDto generationParameter = new GenerationParameterDto(new List<DominionExpansion> { expansion }, 20);

            //Act
            _testee.GenerateSet();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _naviServiceMock.Verify(x => x.NavigateToAsync(                
                    NavigationDestination.GeneratedSet,
                    It.Is<GenerationParameterDto>(parameter => 
                        parameter.PropabilityForColonyAndPlatinum == 20 && 
                        parameter.Expansions.First().Equals(expansion))),
                Times.Once);
        }
    }
}
