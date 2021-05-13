using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessEntities;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.DAO.GenerationProfiles;
using DSG.Validation.GenerationProfiles;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DSG.BusinessComponentsTest.GenerationProfiles
{
    [TestFixture]
    public class GenerationProfileBcTest
    {
        private GenerationProfileBc _testee;
        private Mock<IGenerationProfileDao> _generationProfileDaoMock;
        private Mock<IGenerationProfileValidator> _generationValidatorMock;

        [SetUp]
        public void Setup()
        {
            _testee = new GenerationProfileBc();

            _generationProfileDaoMock = new Mock<IGenerationProfileDao>();
            _testee.GenerationProfileDao = _generationProfileDaoMock.Object;

            _generationValidatorMock = new Mock<IGenerationProfileValidator>();
            _testee.GenerationProfileValidator = _generationValidatorMock.Object;
            _generationValidatorMock.Setup(x => x.ValidateName(It.IsAny<string>())).Returns(new ValidationResult(true, null));
            _generationValidatorMock.Setup(x => x.ValidateNameNoDuplicate(It.IsAny<string>())).Returns(new ValidationResult(true, null));
        }

        [Test]
        public void GetGenerationProfiles_DependentDaoMethodInvoked()
        {
            //Arrange
            _generationProfileDaoMock.Setup(x => x.GetGenerationProfiles()).Returns(new List<GenerationProfile> { new GenerationProfile() });

            //Act
            List<GenerationProfile> result = _testee.GetGenerationProfiles();

            //Assert
            result.Should().HaveCount(1);
            _generationProfileDaoMock.Verify(x => x.GetGenerationProfiles(), Times.Once);
        }

        [Test]
        public void InsertGenerationProfile_NoValidationError_DependentDaoMethodInvoked()
        {
            //Assert
            GenerationProfile profile = new GenerationProfile();

            //Act
            string result = _testee.InsertGenerationProfile(profile);

            //Assert
            _generationProfileDaoMock.Verify(x => x.InsertGenerationProfile(profile), Times.Once);

            result.Should().BeNull();
        }

        [Test]
        public void InsertGenerationProfile_NoValidName_ErrorReturnedDaoNotInvoked()
        {
            //Assert
            GenerationProfile profile = new GenerationProfile { Name = "Test Name" };

            _generationValidatorMock.Setup(x => x.ValidateName(profile.Name)).Returns(new ValidationResult(false, "Name not valid."));

            //Act
            string result = _testee.InsertGenerationProfile(profile);

            //Assert
            _generationProfileDaoMock.Verify(x => x.InsertGenerationProfile(profile), Times.Never);

            result.Should().Be("Name not valid. Therefore the profile could not be saved.");
        }

        [Test]
        public void InsertGenerationProfile_DuplicateName_ErrorReturnedDaoNotInvoked()
        {
            //Assert
            GenerationProfile profile = new GenerationProfile { Name = "Test Name" };

            _generationValidatorMock.Setup(x => x.ValidateNameNoDuplicate(profile.Name)).Returns(new ValidationResult(false, "Name duplicated."));

            //Act
            string result = _testee.InsertGenerationProfile(profile);

            //Assert
            _generationProfileDaoMock.Verify(x => x.InsertGenerationProfile(profile), Times.Never);

            result.Should().Be("Name duplicated. Therefore the profile could not be saved.");
        }

        [Test]
        public void DeleteGenerationProfile_PropabilitiesForNonSupplyRemainForOtherProfile_PropabilitiesNotDeleted()
        {
            // Arrange
            int propabilityId = 2;
            int profileId = 1;
            GenerationProfile generationProfile = new GenerationProfile { Id = profileId, PropabilityForNonSupplyCardsId = propabilityId };

            _generationProfileDaoMock.Setup(x => x.IsPropabilitiesForNonSupplyCardsStillUsed(propabilityId)).Returns(true);

            // Act
            _testee.DeleteGenerationProfile(generationProfile);

            // Assert
            _generationProfileDaoMock.Verify(x => x.DeleteSelectedExpansionToGenerationProfilesByProfileId(profileId), Times.Once);
            _generationProfileDaoMock.Verify(x => x.DeleteGenerationProfile(generationProfile), Times.Once);
            _generationProfileDaoMock.Verify(x => x.IsPropabilitiesForNonSupplyCardsStillUsed(propabilityId), Times.Once);
            _generationProfileDaoMock.Verify(x => x.DeletePropabilityForNonSupplyCardsById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteGenerationProfile_PropabilitiesForNonSupplyNotUsedAnymore_IsDeleted()
        {
            // Arrange
            int propabilityId = 2;
            int profileId = 1;
            GenerationProfile generationProfile = new GenerationProfile { Id = profileId, PropabilityForNonSupplyCardsId = propabilityId };

            _generationProfileDaoMock.Setup(x => x.IsPropabilitiesForNonSupplyCardsStillUsed(propabilityId)).Returns(false);

            // Act
            _testee.DeleteGenerationProfile(generationProfile);

            // Assert
            _generationProfileDaoMock.Verify(x => x.DeleteSelectedExpansionToGenerationProfilesByProfileId(profileId), Times.Once);
            _generationProfileDaoMock.Verify(x => x.DeleteGenerationProfile(generationProfile), Times.Once);
            _generationProfileDaoMock.Verify(x => x.IsPropabilitiesForNonSupplyCardsStillUsed(propabilityId), Times.Once);
            _generationProfileDaoMock.Verify(x => x.DeletePropabilityForNonSupplyCardsById(propabilityId), Times.Once);
        }

        [Test]
        public void SetupInitialGenerationProfile_InsertNull_InitialProfileReturned()
        {
            // Act
            GenerationProfile result = _testee.SetupInitialGenerationProfile(null);

            // Assert
            result.PropabilityForPlatinumAndColony.Should().Be(20);
            result.PropabilityForShelters.Should().Be(10);

            result.PropabilityForNonSupplyCards.PropabilityForOne.Should().Be(50);
            result.PropabilityForNonSupplyCards.PropabilityForTwo.Should().Be(30);
            result.PropabilityForNonSupplyCards.PropabilityForThree.Should().Be(7);
            result.PropabilityForNonSupplyCards.PropabilityForFour.Should().Be(0);
        }

        [Test]
        public void SetupInitialGenerationProfile_InsertExistingProfile_SameProfileReturned()
        {
            // Arrange
            GenerationProfile generationProfile = new GenerationProfile { PropabilityForPlatinumAndColony = 10, PropabilityForShelters = 5 };

            // Act
            GenerationProfile result = _testee.SetupInitialGenerationProfile(generationProfile);

            // Assert
            result.Should().Be(generationProfile);

            result.PropabilityForPlatinumAndColony.Should().Be(10);
            result.PropabilityForShelters.Should().Be(5);
        }

        [Test]
        public void PrepareGenerationProfileForInsertion_WithExistingPropabilitiesForNonSupply_EnrichedWithExistingProp()
        {
            // Arrange
            PropabilityForNonSupplyCards firstPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(40, 30, 20, 10) { Id = 1 };
            GenerationProfile firstProfile = new GenerationProfile(25, 15, firstPropabilityForNonSupplyCards);
            PropabilityForNonSupplyCards secondPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(35, 25, 15, 5) { Id = 2 };
            GenerationProfile secondProfile = new GenerationProfile(15, 5, secondPropabilityForNonSupplyCards);
            IEnumerable<GenerationProfile> existingProfiles = new List<GenerationProfile> { firstProfile, secondProfile };

            PropabilityForNonSupplyCards duplicatedPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(40, 30, 20, 10) { Id = 3 };
            GenerationProfile generationProfile = new GenerationProfile(20, 10, duplicatedPropabilityForNonSupplyCards);

            ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDtos = new ObservableCollection<IsSelectedAndWeightedExpansionDto>();
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()));
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()) { IsSelected = false });

            // Act
            GenerationProfile result = _testee.PrepareGenerationProfileForInsertion(generationProfile, isSelectedAndWeightedExpansionDtos, existingProfiles);

            // Assert
            result.PropabilityForNonSupplyCards.Should().Be(firstPropabilityForNonSupplyCards);
            result.SelectedExpansions.Should().HaveCount(1);
        }

        [Test]
        public void PrepareGenerationProfileForInsertion_WithNewPropabilitiesForNonSupply_NewProp()
        {
            // Arrange
            PropabilityForNonSupplyCards firstPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(40, 30, 20, 10) { Id = 1 };
            GenerationProfile firstProfile = new GenerationProfile(25, 15, firstPropabilityForNonSupplyCards);
            PropabilityForNonSupplyCards secondPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(35, 25, 15, 5) { Id = 2 };
            GenerationProfile secondProfile = new GenerationProfile(15, 5, secondPropabilityForNonSupplyCards);
            IEnumerable<GenerationProfile> existingProfiles = new List<GenerationProfile> { firstProfile, secondProfile };

            PropabilityForNonSupplyCards duplicatedPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(42, 32, 22, 12) { Id = 3 };
            GenerationProfile generationProfile = new GenerationProfile(20, 10, duplicatedPropabilityForNonSupplyCards);

            ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDtos = new ObservableCollection<IsSelectedAndWeightedExpansionDto>();
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()));
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()) { IsSelected = false });

            // Act
            GenerationProfile result = _testee.PrepareGenerationProfileForInsertion(generationProfile, isSelectedAndWeightedExpansionDtos, existingProfiles);

            // Assert
            result.PropabilityForNonSupplyCards.Id.Should().Be(3);
            result.SelectedExpansions.Should().HaveCount(1);
        }

        [Test]
        public void PrepareGenerationProfileForInsertion_WithNoExistingPropabilitiesForNonSupply_EnrichedWithExistingProp()
        {
            // Arrange
            PropabilityForNonSupplyCards firstPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(40, 30, 20, 10) { Id = 1 };
            GenerationProfile firstProfile = new GenerationProfile(25, 15, firstPropabilityForNonSupplyCards);
            PropabilityForNonSupplyCards secondPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(35, 25, 15, 5) { Id = 2 };
            GenerationProfile secondProfile = new GenerationProfile(15, 5, secondPropabilityForNonSupplyCards);
            IEnumerable<GenerationProfile> existingProfiles = new List<GenerationProfile> { firstProfile, secondProfile };

            PropabilityForNonSupplyCards duplicatedPropabilityForNonSupplyCards = new PropabilityForNonSupplyCards(40, 30, 20, 10) { Id = 3 };
            GenerationProfile generationProfile = new GenerationProfile(20, 10, duplicatedPropabilityForNonSupplyCards);

            ObservableCollection<IsSelectedAndWeightedExpansionDto> isSelectedAndWeightedExpansionDtos = new ObservableCollection<IsSelectedAndWeightedExpansionDto>();
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()));
            isSelectedAndWeightedExpansionDtos.Add(new IsSelectedAndWeightedExpansionDto(new DominionExpansion()) { IsSelected = false });

            // Act
            GenerationProfile result = _testee.PrepareGenerationProfileForInsertion(generationProfile, isSelectedAndWeightedExpansionDtos, existingProfiles);

            // Assert
            result.PropabilityForNonSupplyCards.Should().Be(firstPropabilityForNonSupplyCards);
            result.SelectedExpansions.Should().HaveCount(1);
        }
    }
}
