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

            _generationProfileBcMock.Verify(x => x.SetupInitialGenerationProfile(null), Times.Once);

            _testee.GenerationProfiles.Should().HaveCount(2);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSet_NothingDone()
        {
            //Arrange
            GenerationProfile selectedProfile = new GenerationProfile();
            _testee.SelectedProfile = selectedProfile;

            _generationProfileBcMock.Setup(x => x.SetupInitialGenerationProfile(selectedProfile)).Returns(selectedProfile);

            DominionExpansion expansion = new DominionExpansion();
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            _testee.IsDominionExpansionSelectedDtos.Add(new IsSelectedAndWeightedExpansionDto(expansion));
            _testee.IsDominionExpansionSelectedDtos.First().IsSelected = false;

            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity());

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(1);
            _testee.IsDominionExpansionSelectedDtos.Single().DominionExpansion.Should().Be(expansion);
            _testee.IsDominionExpansionSelectedDtos.Single().IsSelected.Should().BeFalse();

            _generationProfileBcMock.Verify(x => x.SetupInitialGenerationProfile(selectedProfile), Times.Once);
            _testee.SelectedProfile.Should().Be(selectedProfile);

            _testee.GenerationProfiles.Should().HaveCount(1);
        }

        [Test]
        public async Task OnPageLoaded_ParameterSetWithDifferentAmountOfExpansions_ExpansionsAddedAndSelected()
        {
            //Arrange
            DominionExpansion expansion = new DominionExpansion { Id = 1 };
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion, new DominionExpansion { Id = 2 } };
            _testee.IsDominionExpansionSelectedDtos.Add(new IsSelectedAndWeightedExpansionDto(expansion));
            _testee.IsDominionExpansionSelectedDtos.First().IsSelected = false;

            //Act
            await _testee.OnPageLoadedAsync(expansions);

            //Assert
            _testee.IsDominionExpansionSelectedDtos.Should().HaveCount(2);
            _testee.IsDominionExpansionSelectedDtos[0].IsSelected.Should().BeFalse();
            _testee.IsDominionExpansionSelectedDtos[1].IsSelected.Should().BeTrue();
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

            _testee.IsDominionExpansionSelectedDtos.Add(new IsSelectedAndWeightedExpansionDto(expansion));

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

            _testee.IsDominionExpansionSelectedDtos.Add(new IsSelectedAndWeightedExpansionDto(expansion));

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
            GenerationProfile newProfile = new GenerationProfile(2, 20, TestDataDefines.PropabilitiesForNonSupplyCards.Zero) { Name = "Cloned Profile" };
            _testee.SelectedProfile = newProfile;

            _generationProfileBcMock.Setup(x => x.PrepareGenerationProfileForInsertion(
                _testee.SelectedProfile, _testee.IsDominionExpansionSelectedDtos, It.IsAny<IEnumerable<GenerationProfile>>()))
                .Returns(newProfile);

            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity());

            //Act
            _testee.SaveProfile();

            //Assert
            _generationProfileBcMock.Verify(
                    x => x.InsertGenerationProfile(It.Is<GenerationProfile>(
                        profile => profile.Name == "Cloned Profile")), 
                Times.Once);
            _generationProfileBcMock.Verify(x => x.InsertGenerationProfile(_testee.SelectedProfile), Times.Never, "Should be a cloned entity");
            _testee.GenerationProfiles.Should().HaveCount(2);

            _testee.SelectedProfile.Should().NotBe(newProfile);
        }

        [Test]
        public void SaveProfile_ValidationFails_MessageBoxInvokedNoProfileAdded()
        {
            //Arrange
            GenerationProfile selectedProfileBeforeSaving = new GenerationProfile(10, 20, TestDataDefines.PropabilitiesForNonSupplyCards.Zero);
            _testee.SelectedProfile = selectedProfileBeforeSaving;
            GenerationProfile newProfile = new GenerationProfile { Name = "Cloned Profile" };

            _generationProfileBcMock.Setup(x => x.PrepareGenerationProfileForInsertion(
                _testee.SelectedProfile, _testee.IsDominionExpansionSelectedDtos, It.IsAny<IEnumerable<GenerationProfile>>()))
                .Returns(newProfile);
            _generationProfileBcMock.Setup(x => x.InsertGenerationProfile(It.Is<GenerationProfile>(y => y.Name == "Cloned Profile"))).Returns("Error");

            //Act
            _testee.SaveProfile();

            //Assert
            _generationProfileBcMock.Verify(
                    x => x.InsertGenerationProfile(It.Is<GenerationProfile>(
                        profile => profile.Name == "Cloned Profile")), 
                Times.Once);
            _testee.GenerationProfiles.Should().HaveCount(0);

            _uiServiceMock.Verify(x => x.ShowErrorMessage("Error", "Profile Invalid!"));

            _testee.SelectedProfile.Should().Be(selectedProfileBeforeSaving);
        }

        [Test]
        public void LoadProfile_ProfileLoaded_NameReset()
        {
            //Arrange
            object profile = new GenerationProfile(2, 20, TestDataDefines.PropabilitiesForNonSupplyCards.Default) { Name = "Loaded Profile" };

            //Act
            _testee.LoadProfile(profile);

            //Assert
            _testee.SelectedProfile.PropabilityForShelters.Should().Be(2);
            _testee.SelectedProfile.PropabilityForPlatinumAndColony.Should().Be(20);
        }

        [Test]
        public void DeleteProfile_TwoProfilesAvailable_OnlyOneAvailableAndBcDeletionInvoked()
        {
            // Arrange
            GenerationProfile profile1 = new GenerationProfile { Id = 1, Name = "Profile 1" };
            GenerationProfile profile2 = new GenerationProfile { Id = 2, Name = "To be deleted profile" };

            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity(profile1, _messengerMock.Object, new ObservableCollection<IsSelectedAndWeightedExpansionDto>()));
            _testee.GenerationProfiles.Add(new GenerationProfileViewEntity(profile2, _messengerMock.Object, new ObservableCollection<IsSelectedAndWeightedExpansionDto>()));

            //Act
            _testee.DeleteProfile(profile2);

            //Assert
            _generationProfileBcMock.Verify(x => x.DeleteGenerationProfile(profile2), Times.Once);
            _testee.GenerationProfiles.Should().HaveCount(1);
            _testee.GenerationProfiles.Single().GenerationProfile.Name.Should().Be("Profile 1");
        }
    }
}
