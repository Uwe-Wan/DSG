using System.Collections.Generic;
using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using FluentAssertions;
using NUnit.Framework;

namespace DSG.BusinessComponentsTest.Generation
{
    [TestFixture]
    public class SetGeneratorBcTest
    {
        private SetGeneratorBc _testee;

        [SetUp]
        public void Setup()
        {
            _testee = new SetGeneratorBc();
        }

        [Test]
        public void GenerateSet_LessThan10AvailableCards_ReturnNull()
        {
            //Arrange
            Card oneCard = new Card();
            List<Card> cards = new List<Card> { oneCard };

            //Act
            List<Card> result = _testee.GenerateSet(cards);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void GenerateSet_15AvailableCards_SetOf10Returned()
        {
            //Arrange
            List<Card> availableCards = new List<Card>();
            for (int i= 0; i< 15; i++)
            {
                availableCards.Add(new Card());
            }

            //Act
            List<Card> result = _testee.GenerateSet(availableCards);

            //Assert
            result.Should().HaveCount(10);
        }
    }
}
