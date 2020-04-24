using System.Collections.Generic;
using DSG.BusinessComponents.Expansions;
using DSG.BusinessEntities;
using DSG.Presentation.ViewModel;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using DSG.Presentation.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class ManageSetsViewModelTest
    {
        private ManageSetsViewModel _testee;
        private Mock<IDominionExpansionBc> _dominionExpansionBcMock;
        private Mock<INaviService> _naviServiceMock;

        [SetUp]
        public void Setup()
        {
            _testee = new ManageSetsViewModel();

            _dominionExpansionBcMock = new Mock<IDominionExpansionBc>();
            _testee.DominionExpansionBc = _dominionExpansionBcMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            _testee.DominionExpansions = new ObservableCollection<DominionExpansion>();
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
            //Arrange
            _testee.DominionExpansions = new ObservableCollection<DominionExpansion>();

            List<DominionExpansion> expansions = new List<DominionExpansion> { new DominionExpansion() };
            _dominionExpansionBcMock.Setup(bc => bc.GetExpansions()).Returns(expansions);

            //Act
            await _testee.OnPageLoadedAsync(NavigationDestination.ManageSets);

            //Assert
            _dominionExpansionBcMock.Verify(bc => bc.GetExpansions(), Times.Once);
            _testee.DominionExpansions.Should().HaveCount(1);
        }

        [Test]
        public void AddCards_NaviServiceInvoked()
        {
            //Arrange
            DominionExpansion selectedExpansion = new DominionExpansion();
            _testee.SelectedExpansion = selectedExpansion;

            ObservableCollection<DominionExpansion> expansions = new ObservableCollection<DominionExpansion> { selectedExpansion };
            _testee.DominionExpansions = expansions;

            //Act
            _testee.AddCards();

            //Assert
            _naviServiceMock.Verify(mock => mock.NavigateToAsync(NavigationDestination.ManageCards, selectedExpansion, expansions));
        }
    }
}
