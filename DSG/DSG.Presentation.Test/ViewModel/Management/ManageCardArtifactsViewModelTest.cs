using DSG.BusinessComponents.CardArtifacts;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Presentation.ViewEntity;
using DSG.Presentation.ViewModel.Management;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel.Management
{
    [TestFixture]
    public class ManageCardArtifactsViewModelTest
    {
        private ManageCardArtifactViewModel _testee;
        private Mock<ICardArtifactBc> _cardArtifactBcMock;

        [Test]
        public void InsertCard_BcInvokedAndAddedToCollection()
        {
            //Arrange
            _testee = new ManageCardArtifactViewModel();

            _cardArtifactBcMock = new Mock<ICardArtifactBc>();
            _testee.CardArtifactBc = _cardArtifactBcMock.Object;

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion());

            _testee.NameOfNewArtifact = "TestArtifact";

            //Act
            _testee.AddArtifact();

            //Assert
            _cardArtifactBcMock.Verify(x => x.InsertArtifact(It.Is<CardArtifact>(artifact => artifact.Name == "TestArtifact")), Times.Once);
            _testee.SelectedExpansionViewEntity.ContainedArtifacts.Should().HaveCount(1);
            _testee.SelectedExpansionViewEntity.ContainedArtifacts.First().Name.Should().Be("TestArtifact");
        }

        [Test]
        public async Task OnPageLoadedAsync_NewExpansionViewEntityAddedOldRemoved()
        {
            //Arrange
            _testee = new ManageCardArtifactViewModel();

            SelectedExpansionViewEntity selectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { Name = "OldExpansion" });
            _testee.SelectedExpansionViewEntity = selectedExpansionViewEntity;

            AdditionalCard additionalCardToRemove = new AdditionalCard { MaxCosts = 2 };
            _testee.AdditionalCards.Add(additionalCardToRemove);

            DominionExpansion newExpansion = new DominionExpansion { Name = "NewExpansion" };

            AdditionalCard additionalCardToAdd = new AdditionalCard { MaxCosts = 1 };
            CardArtifact cardArtifactToAdd = new CardArtifact { AdditionalCard = additionalCardToAdd };
            List<DominionExpansion> newExpansions = new List<DominionExpansion> 
            {
                new DominionExpansion 
                {
                    ContainedArtifacts = new List<CardArtifact> 
                    {
                        cardArtifactToAdd
                    } 
                } 
            };

            //Act
            await _testee.OnPageLoadedAsync(newExpansion, newExpansions);

            //Assert
            _testee.SelectedExpansionViewEntity.ExpansionName.Should().Be("NewExpansion");

            _testee.AdditionalCards.Should().HaveCount(1);
            _testee.AdditionalCards.Should().NotContain(additionalCardToRemove);
            _testee.AdditionalCards.Should().Contain(additionalCardToAdd);
        }
    }
}
