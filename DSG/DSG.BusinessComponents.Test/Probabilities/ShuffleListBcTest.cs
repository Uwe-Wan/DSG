using DSG.BusinessComponents.Probabilities;
using DSG.BusinessEntities;
using DSG.Common.Provider;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DSG.BusinessComponentsTest.Probabilities
{
    [TestFixture]
    public class ShuffleListBcTest
    {
        private ShuffleListBc<int> _testee;
        private Mock<IRandomProvider> _randomMock;

        [Test]
        public void ReturnGivenNumberOfRandomElementsFromList_ListOfFiveReturnTwo_RandomMockedForThreeAndFive()
        {
            //Arrange
            _testee = new ShuffleListBc<int>();

            _randomMock = new Mock<IRandomProvider>();
            _testee.Random = _randomMock.Object;
            //return two for the third element of the list and three for the fourth which was initially the fifth before removing the third element
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(5)).Returns(2);
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(4)).Returns(3);

            List<int> integers = new List<int> { 1, 2, 3, 4, 5 };

            //Act
            List<int> result = _testee.ReturnGivenNumberOfRandomElementsFromList(integers, 2);

            //Assert
            result.Should().HaveCount(2);
            result.Should().Contain(3);
            result.Should().Contain(5);
        }
    }
}
