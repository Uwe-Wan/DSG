using System;
using DSG.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace DSG.Common.Test
{
    [TestFixture]
    public class CheckTest
    {
        [Test]
        public void RequireInjected_NotInjected_ThrowsException()
        {
            //Arrange
            string objectToTest = null;
            string parent = "Parent";

            //Act
            Action act = () => Check.RequireInjected(objectToTest, nameof(objectToTest), parent);

            //Assert
            act.Should().Throw<TechnicalException>().WithMessage("The objectToTest was not correctly injected into Parent.");
        }

        [Test]
        public void RequireInjected_Injected_NoException()
        {
            //Arrange
            string objectToTest = "NotNull";
            string parent = "Parent";

            //Act
            Action act = () => Check.RequireInjected(objectToTest, nameof(objectToTest), parent);

            //Assert
            act.Should().NotThrow<TechnicalException>();
        }

        [Test]
        public void RequireNotNull_Null_ThrowsException()
        {
            //Arrange
            string objectToTest = null;
            string parent = "Parent";

            //Act
            Action act = () => Check.RequireNotNull(objectToTest, nameof(objectToTest), parent);

            //Assert
            act.Should().Throw<TechnicalException>().WithMessage("The objectToTest in Parent is null but must not be.");
        }

        [Test]
        public void RequireNotNull_NotNull_NoException()
        {
            //Arrange
            string objectToTest = "NotNull";
            string parent = "Parent";

            //Act
            Action act = () => Check.RequireNotNull(objectToTest, nameof(objectToTest), parent);

            //Assert
            act.Should().NotThrow<TechnicalException>();
        }
    }
}
