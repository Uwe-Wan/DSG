using DSG.BusinessComponents.CardAttributes;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardAttributes;
using DSG.Presentation.ViewModel.Generation;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel
{
    [TestFixture]
    public class ManageCardArtifactsViewModelTest
    {
        private ManageCardArtifactViewModel _testee;
        private Mock<ICardAttributeBc> _cardArtifactBcMock;

        [Test]
        public void InsertCard_BcInvokedAndAddedToCollection()
        {
            //Arrange
            _testee = new ManageCardArtifactViewModel();

            _cardArtifactBcMock = new Mock<ICardAttributeBc>();
            _testee.CardAttributeBc = _cardArtifactBcMock.Object;

            CardAttribute artifact = new CardAttribute();
            _testee.CardArtifactToInsert = artifact;

            //Act
            _testee.AddArtifact();

            //Assert
            _cardArtifactBcMock.Verify(x => x.InsertAttribute(artifact), Times.Once);
            _testee.CardArtifacts.Should().HaveCount(1);
            _testee.CardArtifacts.First().Should().Be(artifact);
        }

        [Test]
        public async Task OnPageLoadedAsync_NewArtifactAddedOldRemoved()
        {
            //Arrange
            _testee = new ManageCardArtifactViewModel();

            CardAttribute cardArtifactToRemove = new CardAttribute();
            _testee.CardArtifacts.Add(cardArtifactToRemove);

            CardAttribute cardArtifactToAdd = new CardAttribute();
            List<CardAttribute> newArtifacts = new List<CardAttribute> { cardArtifactToAdd };
            List<DominionExpansion> expansionsWithNewArtifacts = new List<DominionExpansion> { new DominionExpansion { ContainedCards = new List<Card> { new Card { CardAttributes = newArtifacts } } } };

            //Act
            await _testee.OnPageLoadedAsync(expansionsWithNewArtifacts);

            //Assert
            _testee.CardArtifacts.Should().HaveCount(1);
            _testee.CardArtifacts.Should().NotContain(cardArtifactToRemove);
            _testee.CardArtifacts.Should().Contain(cardArtifactToAdd);
        }
    }
}
