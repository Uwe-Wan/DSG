using DSG.BusinessEntities.GenerationProfiles;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DSG.Validation.Test
{
    [TestFixture]
    public class ValidationMethodsTest
    {
        private ValidationMethods _testee;

        [Test]
        public void ValidateStringLength_StringTooLong_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateStringLength("Four", 3);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Name must not be longer than 20 characters.");
        }

        [Test]
        public void ValidateStringLength_StringNotTooLong_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateStringLength("Four", 5);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateStringNotNullNotEmpty_NotNullNotEmpty_ValidResult()
        {
            // Arrange 
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateStringNotNullNotEmpty("Valid");

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateStringNotNullNotEmpty_Null_InvalidResult()
        {
            // Arrange 
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateStringNotNullNotEmpty(null);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Name must be set.");
        }

        [Test]
        public void ValidateStringNotNullNotEmpty_Empty_InvalidResult()
        {
            // Arrange 
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateStringNotNullNotEmpty(string.Empty);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Name must be set.");
        }

        [Test]
        public void ValidateNameNoDuplicate_NoDuplicate_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            IEnumerable<string> existingProfileNames = new List<string> { "Existing 1", "Existing 2" };

            // Act
            ValidationResult result = _testee.ValidateNameNoDuplicate("New Profile", existingProfileNames, nameof(GenerationProfile));

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateNameNoDuplicate_Duplicate_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            IEnumerable<string> existingProfileNames = new List<string> { "Existing 1", "Existing 2" };

            // Act
            ValidationResult result = _testee.ValidateNameNoDuplicate("Existing 1", existingProfileNames, nameof(GenerationProfile));

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("There exists a GenerationProfile with that name.");
        }
    }
}
