using NUnit.Framework;
using DSG.BusinessComponents.Probabilities;
using Moq;
using DSG.Common.Provider;
using FluentAssertions;
using System;
using DSG.Common.Exceptions;

namespace DSG.BusinessComponentsTest.Probabilities
{
    [TestFixture]
    public class GetIntByProbabilityBcTest
    {
        private GetIntByProbabilityBc _testee;
        private Mock<IRandomProvider> _randomProviderMock;

        [Test]
        public void GetRandomIntInBetweenZeroAndInputParameterCount_25PercentForValueOneRandomAs20Mocked_OneReturned()
        {
            //Arrange
            _testee = new GetIntByProbabilityBc();

            _randomProviderMock = new Mock<IRandomProvider>();
            _randomProviderMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(20);
            _testee.Random = _randomProviderMock.Object;

            //Act
            int result = _testee.GetRandomIntInBetweenZeroAndInputParameterCount(25);

            //Assert
            result.Should().Be(1);
        }

        [Test]
        public void GetRandomIntInBetweenZeroAndInputParameterCount_25PercentForValueOne50ForTwoRandomAs60Mocked_TwoReturned()
        {
            //Arrange
            _testee = new GetIntByProbabilityBc();

            _randomProviderMock = new Mock<IRandomProvider>();
            _randomProviderMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(60);
            _testee.Random = _randomProviderMock.Object;

            //Act
            int result = _testee.GetRandomIntInBetweenZeroAndInputParameterCount(25,50);

            //Assert
            result.Should().Be(2);
        }

        [Test]
        public void GetRandomIntInBetweenZeroAndInputParameterCount_25PercentForValueOne50ForTwoRandomAs80Mocked_ZeroReturned()
        {
            //Arrange
            _testee = new GetIntByProbabilityBc();

            _randomProviderMock = new Mock<IRandomProvider>();
            _randomProviderMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(80);
            _testee.Random = _randomProviderMock.Object;

            //Act
            int result = _testee.GetRandomIntInBetweenZeroAndInputParameterCount(25,50);

            //Assert
            result.Should().Be(0);
        }

        [Test]
        public void GetRandomIntInBetweenZeroAndInputParameterCount_25PercentForValueOne80ForTwo_ThrowException()
        {
            //Arrange
            _testee = new GetIntByProbabilityBc();

            _randomProviderMock = new Mock<IRandomProvider>();
            _randomProviderMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(80);
            _testee.Random = _randomProviderMock.Object;

            Action act = () => _testee.GetRandomIntInBetweenZeroAndInputParameterCount(25, 80);

            //Act
            act.Should().Throw<ProbabilitiesTooHighException>().WithMessage("The sum of all entered probabilities is higher than 100%");
        }
    }
}
