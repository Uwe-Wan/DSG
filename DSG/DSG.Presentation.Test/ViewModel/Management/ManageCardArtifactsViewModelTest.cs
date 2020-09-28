using DSG.BusinessComponents.CardArtifacts;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Presentation.Services;
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
        private Mock<INaviService> _naviMock;

        [SetUp]
        public void Setup()
        {
            _testee = new ManageCardArtifactViewModel();

            _cardArtifactBcMock = new Mock<ICardArtifactBc>();
            _testee.CardArtifactBc = _cardArtifactBcMock.Object;

            _naviMock = new Mock<INaviService>();
            _testee.NaviService = _naviMock.Object;
        }

        [Test]
        public void AddArtifact_NoAdditionalCard_BcInvokedWithNoAdditional_AddedToCollection_NewArtifactReset()
        {
            //Arrange
            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion());

            _testee.NewArtifact = new CardArtifact();
            _testee.NewArtifact.Name = "TestArtifact";
            _testee.SelectedAdditionalCardType = TypeOfAdditionalCard.None;
            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { Id = 1 });

            CardArtifact selectedArtifactAfterAdding = new CardArtifact
            {
                AdditionalCard = new AdditionalCard()
            };
            selectedArtifactAfterAdding.AdditionalCardId = selectedArtifactAfterAdding.AdditionalCard.Id;
            selectedArtifactAfterAdding.DominionExpansionId = _testee.SelectedExpansionViewEntity.DominionExpansion.Id;

            //Act
            _testee.AddArtifact();

            //Assert
            _cardArtifactBcMock.Verify(x => x.InsertArtifact(It.Is<CardArtifact>(artifact => 
            artifact.Name == "TestArtifact" &&
            artifact.AdditionalCard == null
            )), Times.Once);

            _testee.SelectedExpansionViewEntity.ContainedArtifacts.Should().HaveCount(1);
            _testee.SelectedExpansionViewEntity.ContainedArtifacts.First().Name.Should().Be("TestArtifact");

            _testee.AdditionalCards.Should().HaveCount(0);

            _testee.NewArtifact.Should().BeEquivalentTo(selectedArtifactAfterAdding);
        }

        [Test]
        [Ignore("DSG-26: ignored till bug is fixed")]
        public void AddArtifact_ExistingCard_AvailableAdditional_BcInvoked_AddedToCollection()
        {
            //Arrange

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion());

            _testee.NewArtifact = new CardArtifact();
            _testee.NewArtifact.Name = "TestArtifact";
            _testee.SelectedAdditionalCardType = TypeOfAdditionalCard.Existing;
            AdditionalCard additionalCard = new AdditionalCard(2, 4, TypeOfAdditionalCard.None);
            _testee.NewArtifact.AdditionalCard = additionalCard;

            _testee.AdditionalCards.Add(
                new AdditionalCard(2, 4, TypeOfAdditionalCard.Additional));
            _testee.AdditionalCards.Add(
                new AdditionalCard(2, 6, TypeOfAdditionalCard.Existing));

            //Act
            _testee.AddArtifact();

            //Assert
            _cardArtifactBcMock.Verify(x => x.InsertArtifact(It.Is<CardArtifact>(artifact => 
            artifact.Name == "TestArtifact" &&
            artifact.AdditionalCard.Equals(additionalCard)
            )), Times.Once);

            _testee.SelectedExpansionViewEntity.ContainedArtifacts.Should().HaveCount(1);
            _testee.SelectedExpansionViewEntity.ContainedArtifacts.First().Name.Should().Be("TestArtifact");

            _testee.AdditionalCards.Should().HaveCount(3);
        }

        [Test]
        [Ignore("DSG-26: ignored till bug is fixed")]
        public void AddArtifact_AdditionalCard_AvailableAdditional_BcInvoked_AddedToCollection()
        {
            //Arrange

            _testee.SelectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion());

            _testee.NewArtifact = new CardArtifact();
            _testee.NewArtifact.Name = "TestArtifact";
            _testee.SelectedAdditionalCardType = TypeOfAdditionalCard.Additional;
            AdditionalCard additionalCard = new AdditionalCard(2, 4, TypeOfAdditionalCard.None);
            _testee.NewArtifact.AdditionalCard = additionalCard;

            _testee.AdditionalCards.Add(
                new AdditionalCard(2, 4, TypeOfAdditionalCard.Existing));
            _testee.AdditionalCards.Add(
                new AdditionalCard(2, 6, TypeOfAdditionalCard.Additional));

            //Act
            _testee.AddArtifact();

            //Assert
            _cardArtifactBcMock.Verify(x => x.InsertArtifact(It.Is<CardArtifact>(artifact => 
            artifact.Name == "TestArtifact" &&
            artifact.AdditionalCard.Equals(additionalCard)
            )), Times.Once);

            _testee.SelectedExpansionViewEntity.ContainedArtifacts.Should().HaveCount(1);
            _testee.SelectedExpansionViewEntity.ContainedArtifacts.First().Name.Should().Be("TestArtifact");

            _testee.AdditionalCards.Should().HaveCount(3);
        }

        [Test]
        public async Task OnPageLoadedAsync_NewExpansionViewEntityAddedOldRemoved()
        {
            //Arrange
            SelectedExpansionViewEntity selectedExpansionViewEntity = new SelectedExpansionViewEntity(new DominionExpansion { Name = "OldExpansion" });
            _testee.SelectedExpansionViewEntity = selectedExpansionViewEntity;

            AdditionalCard existingAdditionalCard = new AdditionalCard { MaxCosts = 2 };
            _testee.AdditionalCards.Add(existingAdditionalCard);

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
            _testee.ManageCardArtifactsScreenTitle.Should().Be("Manage Artifacts of Expansion NewExpansion");

            _testee.AdditionalCards.Should().HaveCount(2);
            _testee.AdditionalCards.Should().Contain(existingAdditionalCard);
            _testee.AdditionalCards.Should().Contain(additionalCardToAdd);
        }

        [Test]
        public async Task NavigateToAsync_NavigationInvoked_DataCleared()
        {
            //Arrange
            _testee.NewArtifact = new CardArtifact();
            _testee.ManageCardArtifactsScreenTitle = "TestTitle";
            _testee.SelectedAdditionalCardType = TypeOfAdditionalCard.Additional;

            //Act
            await _testee.NavigateToAsync(NavigationDestination.WelcomeScreen);

            //Assert
            _naviMock.Verify(x => x.NavigateToAsync(NavigationDestination.WelcomeScreen), Times.Once);

            _testee.ManageCardArtifactsScreenTitle.Should().Be("Manage Artifacts of Expansion ???");
            _testee.SelectedAdditionalCardType.Should().Be(TypeOfAdditionalCard.None);
            _testee.NewArtifact.Should().BeNull();
        }
    }
}
