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

        [SetUp]
        public void Setup()
        {
            _testee = new CostBc();

            _costDaoMock = new Mock<ICostDao>();
            _testee.CostDao = _costDaoMock.Object;
        }

        [Test]
        public void GetCosts_DaoInvoked()
        {
            //Arrange
            Cost two = new Cost(2);
            List<Cost> costs = new List<Cost> { two };

            _costDaoMock.Setup(dao => dao.GetCosts()).Returns(costs);

            //Act
            List<Cost> result = _testee.GetCosts();

            //Assert
            result.Should().HaveCount(1);
            result.Single().Money.Should().Be(2);
        }

        [Test]
        public void InsertCost_DaoInvoked()
        {
            //Arrange
            Cost cost = new Cost();

            //Act
            _testee.InsertCost(cost);

            //Assert
            _costDaoMock.Verify(dao => dao.InsertCost(cost), Times.Once);
        }
    }
}
