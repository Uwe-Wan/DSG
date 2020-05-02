﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using DSG.Presentation.ViewModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class WelcomeScreenViewModelTest
    {
        private WelcomeScreenViewModel _testee;
        private Mock<IUiService> _uiServiceMock;
        private Mock<INaviService> _naviServiceMock;
        private Mock<IDominionExpansionBc> _dominionExpansionBcMock;

        [SetUp]
        public void Setup()
        {
            _testee = new WelcomeScreenViewModel();
            
            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            _dominionExpansionBcMock = new Mock<IDominionExpansionBc>();
            _testee.DominionExpansionBc = _dominionExpansionBcMock.Object;
        }

        [Test]
        public async Task OnPageLoadedAsync_InvokeBc()
        {
            //Arrange
            _testee.DominionExpansions = new ObservableCollection<DominionExpansion>();

            List<DominionExpansion> expansions = new List<DominionExpansion> { new DominionExpansion() };
            _dominionExpansionBcMock.Setup(bc => bc.GetExpansions()).Returns(expansions);

            //Act
            await _testee.OnPageLoadedAsync();

            //Assert
            _dominionExpansionBcMock.Verify(bc => bc.GetExpansions(), Times.Once);
            _testee.DominionExpansions.Should().HaveCount(1);
        }

        [Test]
        public void GenerateSet_NotEnoughCards_OpenUiWindow()
        {
            //Arrange
            _testee = new WelcomeScreenViewModel();

            _uiServiceMock = new Mock<IUiService>();
            _testee.UiService = _uiServiceMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            DominionExpansion expansion = new DominionExpansion { ContainedCards = new List<Card> { new Card() } };

            _testee.DominionExpansions = new ObservableCollection<DominionExpansion> { expansion };

            //Act
            _testee.GoToGenerationOptions();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage("There are only 1 cards available. " +
                    "A minimum of 10 is needed to generate a set.", "Not enough Cards!"), Times.Once);
            _naviServiceMock.Verify(x => x.NavigateToAsync(NavigationDestination.GeneratedSet, It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void GenerateSet_EnoughCards_NavigationInvoked()
        {
            //Arrange
            _testee = new WelcomeScreenViewModel();

            _uiServiceMock = new Mock<IUiService>();
            _testee.UiService = _uiServiceMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            DominionExpansion expansion = new DominionExpansion { ContainedCards = new List<Card>() };

            for (int i = 0; i < 15; i++)
            {
                expansion.ContainedCards.Add(new Card());
            }

            _testee.DominionExpansions = new ObservableCollection<DominionExpansion> { expansion };

            //Act
            _testee.GoToGenerationOptions();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _naviServiceMock.Verify(x => x.NavigateToAsync(NavigationDestination.GenerationOptions, new ObservableCollection<DominionExpansion> { expansion }), Times.Once);
        }
    }
}
