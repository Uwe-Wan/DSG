using System.Collections.Generic;
using System.Linq;
using DSG.BusinessComponents.Costs;
using DSG.BusinessEntities;
using DSG.DAO.Costs;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DSG.BusinessComponentsTest.Costs
{
    [TestFixture]
    public class CostBcTest
    {
        private CostBc _testee;
        private Mock<ICostDao> _costDaoMock;

        [Test]
        public void GetCosts_DaoInvoked()
        {
            //Arrange
            _testee = new CostBc();

            Cost two = new Cost { Money = 2 };
            List<Cost> costs = new List<Cost> { two };

            _costDaoMock = new Mock<ICostDao>();
            _testee.CostDao = _costDaoMock.Object;
            _costDaoMock.Setup(dao => dao.GetCosts()).Returns(costs);

            //Act
            List<Cost> result = _testee.GetCosts();

            //Assert
            result.Should().HaveCount(1);
            result.Single().Money.Should().Be(2);
        }
    }
}
