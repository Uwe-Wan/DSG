using DSG.BusinessEntities.GenerationProfiles;
using DSG.DAO.GenerationProfiles;
using DSG.Validation.GenerationProfiles;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DSG.Validation.Test.GenerationProfiles
{
    [TestFixture]
    public class GenerationProfileValidatorTest
    {
        private GenerationProfileValidator _testee;
        private Mock<IValidationMethods> _validationMethodsMock;
        private Mock<IGenerationProfileDao> _generationProfileDaoMock;

        [SetUp]
        public void Setup()
        {
            _testee = new GenerationProfileValidator();

            _validationMethodsMock = new Mock<IValidationMethods>();
            _testee.ValidationMethods = _validationMethodsMock.Object;

            _generationProfileDaoMock = new Mock<IGenerationProfileDao>();
            _testee.GenerationProfileDao = _generationProfileDaoMock.Object;
        }

        [Test]
        public void ValidateName_ValidName_ValidResult()
        {
            // Arrange
            string validName = "Valid Name";
            _validationMethodsMock.Setup(x => x.ValidateStringLength(validName, 20)).Returns(ValidationResult.ValidResult);
            _validationMethodsMock.Setup(x => x.ValidateStringNotNullNotEmpty(validName)).Returns(ValidationResult.ValidResult);

            // Act
            ValidationResult result = _testee.ValidateName(validName);

            // Assert
            result.IsValid.Should().BeTrue();
            _validationMethodsMock.Verify(x => x.ValidateStringLength(validName, 20), Times.Once);
            _validationMethodsMock.Verify(x => x.ValidateStringNotNullNotEmpty(validName), Times.Once);
        }

        [Test]
        public void ValidateName_TooLong_InvalidResult()
        {
            // Arrange
            string tooLongName = "Way toooooo long Name";
            _validationMethodsMock.Setup(x => x.ValidateStringLength(tooLongName, 20)).Returns(new ValidationResult(false, "Too Long"));
            _validationMethodsMock.Setup(x => x.ValidateStringNotNullNotEmpty(tooLongName)).Returns(ValidationResult.ValidResult);

            // Act
            ValidationResult result = _testee.ValidateName(tooLongName);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Too Long");

            _validationMethodsMock.Verify(x => x.ValidateStringLength(tooLongName, 20), Times.Once);
            _validationMethodsMock.Verify(x => x.ValidateStringNotNullNotEmpty(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ValidateName_Empty_InvalidResult()
        {
            // Arrange
            string emptyName = string.Empty;
            _validationMethodsMock.Setup(x => x.ValidateStringLength(emptyName, 20)).Returns(ValidationResult.ValidResult);
            _validationMethodsMock.Setup(x => x.ValidateStringNotNullNotEmpty(emptyName)).Returns(new ValidationResult(false, "Empty"));

            // Act
            ValidationResult result = _testee.ValidateName(emptyName);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Empty");

            _validationMethodsMock.Verify(x => x.ValidateStringLength(emptyName, 20), Times.Once);
            _validationMethodsMock.Verify(x => x.ValidateStringNotNullNotEmpty(emptyName), Times.Once);
        }

        [Test]
        public void ValidateNameNoDuplicate_ValidationMethodInvoked_WithSameInput()
        {
            // Arrange
            IEnumerable<string> existingNames = new List<string> { "Existing 1", "Existing 2" };
            string profileName = "New Profile";

            // Act
            _testee.ValidateNameNoDuplicate(profileName, existingNames);

            // Assert
            _validationMethodsMock.Verify(x => x.ValidateNameNoDuplicate(profileName, existingNames, "GenerationProfile"), Times.Once);
        }

        [Test]
        public void ValidateNameNoDuplicate_ValidationMethodInvoked_WithExistingFromDao()
        {
            // Arrange
            List<GenerationProfile> existingProfiles = new List<GenerationProfile>
            {
                new GenerationProfile { Name = "Existing 1"},
                new GenerationProfile { Name = "Existing 2"}
            };
            _generationProfileDaoMock.Setup(x => x.GetGenerationProfiles()).Returns(existingProfiles);

            string profileName = "New Profile";

            // Act
            _testee.ValidateNameNoDuplicate(profileName);

            // Assert
            _validationMethodsMock.Verify(x => x.ValidateNameNoDuplicate(profileName, It.Is<IEnumerable<string>>(list => list.Count() == 2), "GenerationProfile"), Times.Once);
        }
    }
}
