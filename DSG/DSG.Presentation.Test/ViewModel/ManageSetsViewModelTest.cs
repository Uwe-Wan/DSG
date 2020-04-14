using System.Collections.Generic;
using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.ViewModel;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using DSG.Presentation.Services;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class ManageSetsViewModelTest
    {
        private ManageSetsViewModel _testee;
        private Mock<IDominionExpansionBc> _dominionExpansionBcMock;

        [SetUp]
        public void Setup()
        {
            _testee = new ManageSetsViewModel();

            _dominionExpansionBcMock = new Mock<IDominionExpansionBc>();
            _testee.DominionExpansionBc = _dominionExpansionBcMock.Object;

            _testee.DominionExpansions = new List<DominionExpansion>();
        }

        [Test]
        public void InsertExpansion_BcInvoked()
        {
            //Arrange
            string newExpansionName = "New Expansion";
            _testee.UserInput = newExpansionName;

            DominionExpansion newExpansion = new DominionExpansion { Name = newExpansionName };
            _dominionExpansionBcMock.Setup(bc => bc.GetExpansionByName(newExpansionName)).Returns(newExpansion);
            List<int> test = new List<int> { 1 };

            //Act
            _testee.InsertExpansion();

            //Assert
            _dominionExpansionBcMock.Verify(bc => bc.InsertExpansion(newExpansionName), Times.Once);
            _dominionExpansionBcMock.Verify(bc => bc.GetExpansionByName(newExpansionName), Times.Once);
            test.Should().HaveCount(1);
        }

        [Test]
        public async Task OnPageLoadedAsync_InvokeBc()
        {
            //Act
            await _testee.OnPageLoadedAsync(NavigationDestination.ManageSets);

            //Assert
            _dominionExpansionBcMock.Verify(bc => bc.GetExpansions(), Times.Once);
        }
    }
}
