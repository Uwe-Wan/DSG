using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.Presentation.Messaging;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using DSG.Presentation.ViewModel.Generation;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DSG.Presentation.Test.ViewModel.Generation
{
    [TestFixture]
    public class GenerationOptionsViewModelTest
    {
        private GenerationOptionsViewModel _testee;
        private Mock<IUiService> _uiServiceMock;
        private Mock<INaviService> _naviServiceMock;
        private Mock<IMessenger> _messengerMock;
        private Mock<IGenerationProfileBc> _generationProfileBcMock;

        [SetUp]
        public void Setup()
        {
            _messengerMock = new Mock<IMessenger>();

            _testee = new GenerationOptionsViewModel(_messengerMock.Object);

            _uiServiceMock = new Mock<IUiService>();
            _testee.UiService = _uiServiceMock.Object;

            _naviServiceMock = new Mock<INaviService>();
            _testee.NaviService = _naviServiceMock.Object;

            _generationProfileBcMock = new Mock<IGenerationProfileBc>();
            _testee.GenerationProfileBc = _generationProfileBcMock.Object;
            _generationProfileBcMock.Setup(x => x.GetGenerationProfiles()).Returns(new List<GenerationProfile>());
        }

        [Test]
        public async Task OnPageLoaded_NoParameterSet_ParameterSet()
        {
            //Arrange
            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            _generationProfileBcMock.Setup(x => x.GetGenerationProfiles()).Returns(new List<GenerationProfile> { new GenerationProfile(), new GenerationProfile() });

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeTrue();

            _testee.SelectedProfile.PropabilityForPlatinumAndColony.Should().Be(20);
            _testee.SelectedProfile.PropabilityForShelters.Should().Be(10);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForOne.Should().Be(50);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForTwo.Should().Be(30);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForThree.Should().Be(7);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForFour.Should().Be(0);
            _testee.SelectedProfile.Name.Should().Be("Insert Name");

            _testee.GenerationProfiles.Should().HaveCount(2);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSet_NothingDone()
        {
            //Arrange
            PropabilityForNonSupplyCards propabilityForNonSupplyCards = new PropabilityForNonSupplyCards(42, 14, 2, 1);
            GenerationProfile selectedProfile = new GenerationProfile(15, 30, propabilityForNonSupplyCards);
            _testee.SelectedProfile = selectedProfile;

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            _testee.IsDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(expansion));
            _testee.IsDominionExpansionSelectedDtos.First().IsSelected = false;

            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity());

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeFalse();

            _testee.SelectedProfile.PropabilityForPlatinumAndColony.Should().Be(30);
            _testee.SelectedProfile.PropabilityForShelters.Should().Be(15);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForOne.Should().Be(42);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForTwo.Should().Be(14);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForThree.Should().Be(2);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForFour.Should().Be(1);

            _testee.GenerationProfiles.Should().HaveCount(1);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSetWithDifferentAmountOfExpansions_ExpansionsAddedAndSelected()
        {
            //Arrange
            PropabilityForNonSupplyCards propabilityForNonSupplyCards = new PropabilityForNonSupplyCards(42, 14, 2, 1);
            GenerationProfile selectedProfile = new GenerationProfile(15, 30, propabilityForNonSupplyCards);
            _testee.SelectedProfile = selectedProfile;

            DominionExpansion expansion = new DominionExpansion { Id = 1 };
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion, new DominionExpansion { Id = 2 } };
            _testee.IsDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(expansion));
            _testee.IsDominionExpansionSelectedDtos.First().IsSelected = false;

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(2);
            _testee.IsDominionExpansionSelectedDtos[0].IsSelected.Should().BeFalse();
            _testee.IsDominionExpansionSelectedDtos[1].IsSelected.Should().BeTrue();

            _testee.SelectedProfile.PropabilityForPlatinumAndColony.Should().Be(30);
            _testee.SelectedProfile.PropabilityForShelters.Should().Be(15);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForOne.Should().Be(42);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForTwo.Should().Be(14);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForThree.Should().Be(2);
            _testee.SelectedProfile.PropabilityForNonSupplyCards.PropabilityForFour.Should().Be(1);
        }

        [Test]
        public void GenerateSet_NotEnoughCards_OpenUiWindow()
        {
            //Arrange
            CardTypeToCard supplyType = new CardTypeToCard { CardType = new CardType { IsSupplyType = true } };

            DominionExpansion expansion = new DominionExpansion
            {
                ContainedCards = new List<Card>
                {
                    new Card { CardTypeToCards = new List<CardTypeToCard> { supplyType } }
                }
            };

            _testee.IsDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(expansion));

            _testee.SelectedProfile = new GenerationProfile(20, 10, new PropabilityForNonSupplyCards());

            //Act
            _testee.GenerateSet();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage("There are only 1 cards available in the selected Sets. " +
                    "A minimum of 10 is needed to generate a set.", "Not enough Cards!"), Times.Once);
            _naviServiceMock.Verify(x => x.NavigateToAsync(NavigationDestination.GeneratedSet, It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void GenerateSet_EnoughCards_NavigationInvoked()
        {
            //Arrange
            DominionExpansion expansion = new DominionExpansion { ContainedCards = new List<Card>() };
            CardTypeToCard supplyType = new CardTypeToCard { CardType = new CardType { IsSupplyType = true } };

            for (int i = 0; i < 15; i++)
            {
                expansion.ContainedCards.Add(
                    new Card { CardTypeToCards = new List<CardTypeToCard> { supplyType } });
            }

            _testee.IsDominionExpansionSelectedDtos.Add(new IsDominionExpansionSelectedDto(expansion));

            _testee.SelectedProfile = new GenerationProfile(10, 20, new PropabilityForNonSupplyCards(20, 10, 0, 0));

            //Act
            _testee.GenerateSet();

            //Assert
            _uiServiceMock.Verify(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _naviServiceMock.Verify(x => x.NavigateToAsync(                
                    NavigationDestination.GeneratedSet,
                    It.Is<GenerationParameterDto>(parameter => 
                        parameter.PropabilityForColonyAndPlatinum == 20 && 
                        parameter.Expansions.First().Equals(expansion) &&
                        parameter.PropabilitiesForNonSupplies.PropabilityForOne == 20)),
                Times.Once);
        }

        [Test]
        public void SaveProfile_BcInvoked_ProfileAddedToCollection()
        {
            //Arrange
            _testee.SelectedProfile = new GenerationProfile { Name = "Test" };

            DominionExpansion world = new DominionExpansion { Name = "World" };
            IsDominionExpansionSelectedDto selectedExpansion = new IsDominionExpansionSelectedDto(world);
            
            DominionExpansion seaside = new DominionExpansion { Name = "Seaside" };
            IsDominionExpansionSelectedDto unselectedExpansion = new IsDominionExpansionSelectedDto(seaside) { IsSelected = false };

            _testee.IsDominionExpansionSelectedDtos.Add(selectedExpansion);
            _testee.IsDominionExpansionSelectedDtos.Add(unselectedExpansion);

            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity());

            //Act
            _testee.SaveProfile();

            //Assert
            _generationProfileBcMock.Verify(
                    x => x.InsertGenerationProfile(It.Is<GenerationProfile>(
                        profile => profile.Name == "Test"
                        && profile.SelectedExpansions.Count == 1)), 
                Times.Once);
            _generationProfileBcMock.Verify(x => x.InsertGenerationProfile(_testee.SelectedProfile), Times.Never, "Should be a cloned entity");
            _testee.GenerationProfiles.Should().HaveCount(2);
        }

        [Test]
        public void LoadProfile_ProfileLoaded_NameReset()
        {
            //Arrange
            object profile = new GenerationProfile { Name = "Loaded Profile", PropabilityForPlatinumAndColony = 20, PropabilityForShelters = 2 };

            //Act
            _testee.LoadProfile(profile);

            //Assert
            _testee.SelectedProfile.Name.Should().Be("Insert Name");
            _testee.SelectedProfile.PropabilityForShelters.Should().Be(2);
            _testee.SelectedProfile.PropabilityForPlatinumAndColony.Should().Be(20);
        }
    }
}
