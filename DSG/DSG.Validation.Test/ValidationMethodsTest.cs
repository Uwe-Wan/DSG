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

        [Test]
        public void ValidateConvertibleToInteger_ConvertibleToInteger_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateConvertibleToInteger("10");

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateConvertibleToInteger_Null_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateConvertibleToInteger((string)null);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Value is no integer.");
        }

        [Test]
        public void ValidateConvertibleToInteger_IsLetter_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateConvertibleToInteger("a");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Value is no integer.");
        }

        [Test]
        public void ValidateIntegerValueNotBigger_HalfOfBoarder_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotBigger(50, 100);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateIntegerValueNotBigger_EqualsBoarder_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotBigger(100, 100);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateIntegerValueNotBigger_ToBig_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotBigger(101, 100);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Value must not be bigger than 100");
        }

        [Test]
        public void ValidateIntegerValueNotSmaller_HalfOfBoarder_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotSmaller(-50, -100);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateIntegerValueNotSmaller_EqualsBoarder_ValidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotSmaller(-100, -100);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateIntegerValueNotSmaller_ToSmall_InvalidResult()
        {
            // Arrange
            _testee = new ValidationMethods();

            // Act
            ValidationResult result = _testee.ValidateIntegerValueNotSmaller(-101, -100);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Value must not be smaller than -100");
        }
    }
}
