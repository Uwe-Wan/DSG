using DSG.BusinessComponents.GenerationProfiles;
using DSG.BusinessEntities.GenerationProfiles;
using DSG.DAO.GenerationProfiles;
using DSG.Validation.GenerationProfiles;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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
        public void DeleteGenerationProfile_DependentDaoMethodInvoked()
        {
            // Arrange
            GenerationProfile generationProfile = new GenerationProfile();

            // Act
            _testee.DeleteGenerationProfile(generationProfile);

            // Assert
            _generationProfileDaoMock.Verify(x => x.DeleteGenerationProfile(generationProfile), Times.Once);
        }
    }
}
