using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using DSG.BusinessComponents.Cards;
using DSG.BusinessEntities;
using DSG.DAO.Cards;
using Moq;

namespace DSG.BusinessComponentsTest.Cards
{
    [TestFixture]
    public class CardBcTest
    {
        private CardBc _testee;
        private Mock<ICardDao> _cardDaoMock;

        [Test]
        public void InsertCard_DaoInvoked()
        {
            //Arrange
            _testee = new CardBc();

            _cardDaoMock = new Mock<ICardDao>();
            _testee.CardDao = _cardDaoMock.Object;

            Card card = new Card();

            //Act
            _testee.InsertCard(card);

            //Assert
            _cardDaoMock.Verify(dao => dao.InsertCard(card), Times.Once);
        }
    }
}
