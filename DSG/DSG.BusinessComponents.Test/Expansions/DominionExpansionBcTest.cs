using System;
using DSG.BusinessComponents.Expansions;
using DSG.DAO.Expansions;
using Moq;
using NUnit.Framework;

namespace DSG.BusinessComponents.Test.Expansions
{
    [TestFixture]
    public class DominionExpansionBcTest
    {
        private DominionExpansionBc _testee;
        Mock<IDominionExpansionDao> _dominionExpansionDaoMock;

        [SetUp]
        public void Setup()
        {
            _testee = new DominionExpansionBc();

            _dominionExpansionDaoMock = new Mock<IDominionExpansionDao>();
            _testee.DominionExpansionDao = _dominionExpansionDaoMock.Object;
        }

        [Test]
        public void GetExpansions_InvokeDao()
        {
            //Act
            _testee.GetExpansions();

            //Assert
            _dominionExpansionDaoMock.Verify(dao => dao.GetExpansions(), Times.Once);
        }

        [Test]
        public void GetExpansionByName_InvokeDao()
        {
            //Arrange
            string randomName = "random Name";

            //Act
            _testee.GetExpansionByName(randomName);

            //Assert
            _dominionExpansionDaoMock.Verify(dao => dao.GetExpansionByName(randomName), Times.Once);
        }

        [Test]
        public void InsertExpansion_InvokeDao()
        {
            //Arrange
            string expansionName = "New Expansion";

            //Act
            _testee.InsertExpansion(expansionName);

            //Assert
            _dominionExpansionDaoMock.Verify(dao => dao.InsertExpansion(expansionName), Times.Once);
        }
    }
}
