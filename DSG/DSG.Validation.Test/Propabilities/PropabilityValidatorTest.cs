using DSG.Validation.Propabilities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DSG.Validation.Test.Propabilities
{
    [TestFixture]
    public class PropabilityValidatorTest
    {
        private PropabilityValidator _testee;
        private Mock<IValidationMethods> _validationMethods;

        [SetUp]
        public void Setup()
        {
            _testee = new PropabilityValidator();

            _validationMethods = new Mock<IValidationMethods>();
            _testee.ValidationMethods = _validationMethods.Object;

            _validationMethods.Setup(x => x.ValidateIntegerValueNotBigger(It.IsAny<int>(), 100)).Returns(ValidationResult.ValidResult);
            _validationMethods.Setup(x => x.ValidateIntegerValueNotSmaller(It.IsAny<int>(), 0)).Returns(ValidationResult.ValidResult);
        }

        [Test]
        public void ValidatePropability_ValidResult_AllValidationsInvoked()
        {
            // Act
            ValidationResult result = _testee.ValidatePropability(50);

            // Assert
            result.IsValid.Should().BeTrue();

            _validationMethods.Verify(x => x.ValidateIntegerValueNotBigger(50, 100), Times.Once);
            _validationMethods.Verify(x => x.ValidateIntegerValueNotSmaller(50, 0), Times.Once);
        }

        [Test]
        public void ValidatePropability_ValueTooBig_SmallerVerificationNotInvoked()
        {
            // Arrange
            _validationMethods.Setup(x => x.ValidateIntegerValueNotBigger(101, 100)).Returns(new ValidationResult(false, "Invalid Result"));

            // Act
            ValidationResult result = _testee.ValidatePropability(101);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Invalid Result");

            _validationMethods.Verify(x => x.ValidateIntegerValueNotBigger(101, 100), Times.Once);
            _validationMethods.VerifyNoOtherCalls();
        }

        [Test]
        public void ValidatePropability_ValueTooSmall_LastResultReturned()
        {
            // Arrange
            _validationMethods.Setup(x => x.ValidateIntegerValueNotSmaller(-1, 0)).Returns(new ValidationResult(false, "Invalid Result"));

            // Act
            ValidationResult result = _testee.ValidatePropability(-1);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorContent.ToString().Should().Be("Invalid Result");

            _validationMethods.Verify(x => x.ValidateIntegerValueNotBigger(-1, 100), Times.Once);
            _validationMethods.Verify(x => x.ValidateIntegerValueNotSmaller(-1, 0), Times.Once);
        }
    }
}
