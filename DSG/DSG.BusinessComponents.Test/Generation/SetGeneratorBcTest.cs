using System;
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

            DominionExpansion expansion = new DominionExpansion { ContainedCards = cards };
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            Action act = () => _testee.GenerateSet(expansions);

            //Act
            act.Should().Throw<Exception>().WithMessage("Not enough Cards available.");
        }

        [Test]
        public void GenerateSet_15AvailableCards_SetOf10Returned()
        {
            //Arrange
            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                for (int i = 0; i < 5; i++)
                {
                    expansion.ContainedCards.Add(new Card());
                }

                expansions.Add(expansion);
            }

            //Act
            List<Card> result = _testee.GenerateSet(expansions);

            //Assert
            result.Should().HaveCount(10);
        }
    }
}
