using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
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
        public async Task OnPageLoaded_BcInvokedWithInputExpansions()
        {
            //Arrange
            List<DominionExpansion> expansions = new List<DominionExpansion> { new DominionExpansion() };

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _setGeneratorBcMock.Verify(x => x.GenerateSet(expansions), Times.Once);
        }

        [Test]
        public async Task NavigateToAsync_NavigationInvoked_DataCleared()
        {
            //Arrange
            _testee.GeneratedSet = new List<Card> { new Card() };

            //Act
            await _testee.NavigateToAsync(NavigationDestination.WelcomeScreen);

            //Assert
            _testee.GeneratedSet.Should().BeNull();

            _naviMock.Verify(x => x.NavigateToAsync(NavigationDestination.WelcomeScreen), Times.Once);
        }
    }
}
