using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using DSG.Presentation.ViewModel.Generation;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel.Generation
{
    [TestFixture]
    public class GeneratedSetViewModelTest
    {
        private GeneratedSetViewModel _testee;
        private Mock<ISetGeneratorBc> _setGeneratorBcMock;
        private Mock<INaviService> _naviMock;

        [SetUp]
        public void Setup()
        {
            _testee = new GeneratedSetViewModel();

            _setGeneratorBcMock = new Mock<ISetGeneratorBc>();
            _testee.SetGeneratorBc = _setGeneratorBcMock.Object;

            _naviMock = new Mock<INaviService>();
            _testee.NaviService = _naviMock.Object;
        }


        [Test]
        public async Task OnPageLoaded_ExpansionsLoaded_PropertiesSet()
        {
            //Arrange
            List<DominionExpansion> expansions = new List<DominionExpansion> { new DominionExpansion() };
            GeneratedSetDto generatedSetDto = new GeneratedSetDto(
                new List<Card> { TestDataDefines.Cards.BridgeTroll }, 
                new List<Card> { TestDataDefines.Cards.Plan }, 
                new List<GeneratedAdditionalCard> { TestDataDefines.GeneratedAdditionalCards.Relic },
                new List<GeneratedAdditionalCard> { TestDataDefines.GeneratedAdditionalCards.Ranger });
            generatedSetDto.HasPlatinumAndColony = true;
            _setGeneratorBcMock.Setup(x => x.GenerateSet(expansions)).Returns(generatedSetDto);

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _setGeneratorBcMock.Verify(x => x.GenerateSet(expansions), Times.Once);
            _testee.SupplyCards.Should().HaveCount(3);
            _testee.SupplyCards.First().Name.Should().Be("Bridge Troll");
            _testee.SupplyCards[1].Name.Should().Be("Ranger");
            _testee.SupplyCards[2].Name.Should().Be("Relic");
            _testee.NonSupplyStuff.Should().HaveCount(5);
            _testee.NonSupplyStuff.Select(x => x.Name).Should().Contain("Plan");
            _testee.NonSupplyStuff.Select(x => x.Name).Should().Contain("-1 Coin Marker");
            _testee.NonSupplyStuff.Select(x => x.Name).Should().Contain("-1 Card Marker");
            _testee.NonSupplyStuff.Select(x => x.Name).Should().Contain("Journey Token");
            _testee.NonSupplyStuff.Select(x => x.Name).Should().Contain("Trash Token");
            _testee.ContainsSheltersOrColony.Should().Be("This set is also played with Colony and Platinum");
        }

        [Test]
        public async Task NavigateToAsync_NavigationInvoked_DataCleared()
        {
            //Arrange
            _testee.SupplyCards = new List<CardAndArtifactViewEntity>();

            //Act
            await _testee.NavigateToAsync(NavigationDestination.WelcomeScreen);

            //Assert
            _testee.SupplyCards.Should().BeNull();

            _naviMock.Verify(x => x.NavigateToAsync(NavigationDestination.WelcomeScreen), Times.Once);
        }
    }
}
