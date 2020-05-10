using DSG.BusinessComponents.CardArtifacts;
using DSG.BusinessEntities.CardArtifacts;
using DSG.DAO.CardArtifacts;
using Moq;
using NUnit.Framework;

namespace DSG.BusinessComponentsTest.CardArtifacts
{
    [TestFixture]
    public class CardArtifactBcTest
    {
        private CardArtifactBc _testee;
        private Mock<ICardArtifactDao> _cardArtifactDaoMock;

        [Test]
        public void InsertCard_DaoInvoked()
        {
            //Arrange
            _testee = new CardArtifactBc();

            _cardArtifactDaoMock = new Mock<ICardArtifactDao>();
            _testee.CardArtifactDao = _cardArtifactDaoMock.Object;

            CardArtifact cardAttribute = new CardArtifact();

            //Act
            _testee.InsertArtifact(cardAttribute);

            //Assert
            _cardArtifactDaoMock.Verify(x => x.InsertArtifact(cardAttribute), Times.Once);
        }
    }
}
