using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.Presentation.ViewModel.Generation;
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

        [Test]
        public async Task OnPageLoaded_BcInvokedWithInputExpansions()
        {
            //Arrange
            _testee = new GeneratedSetViewModel();

            _setGeneratorBcMock = new Mock<ISetGeneratorBc>();
            _testee.SetGeneratorBc = _setGeneratorBcMock.Object;

            List<DominionExpansion> expansions = new List<DominionExpansion> { new DominionExpansion() };

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _setGeneratorBcMock.Verify(x => x.GenerateSet(expansions), Times.Once);
        }
    }
}
