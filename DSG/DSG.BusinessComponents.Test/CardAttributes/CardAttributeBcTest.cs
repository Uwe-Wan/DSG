using System;
using DSG.BusinessComponents.CardAttributes;
using DSG.BusinessEntities.CardArtifacts;
using DSG.DAO.CardAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace DSG.BusinessComponentsTest.CardAttributes
{
    [TestFixture]
    public class CardAttributeBcTest
    {
        private CardAttributeBc _testee;
        private Mock<ICardAttributeDao> _cardAttributeDaoMock;

        [Test]
        public void InsertCard_DaoInvoked()
        {
            //Arrange
            _testee = new CardAttributeBc();

            _cardAttributeDaoMock = new Mock<ICardAttributeDao>();
            _testee.CardAttributeDao = _cardAttributeDaoMock.Object;

            CardArtifact cardAttribute = new CardArtifact();

            //Act
            _testee.InsertAttribute(cardAttribute);

            //Assert
            _cardAttributeDaoMock.Verify(x => x.InsertAttribute(cardAttribute), Times.Once);
        }
    }
}
