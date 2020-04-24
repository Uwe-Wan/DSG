using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.Services;
using DSG.Presentation.ViewModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class WelcomeScreenViewModelTest
    {
        private WelcomeScreenViewModel _testee;
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
    }
}
