using System;
using System.Collections.Generic;
using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
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
            Card oneCard = new Card { CardTypeToCards = new List<CardTypeToCard> { } };
            List<Card> cards = new List<Card> { oneCard };

            DominionExpansion expansion = new DominionExpansion { ContainedCards = cards };
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            Action act = () => _testee.GenerateSet(expansions);

            //Act
            act.Should().Throw<Exception>().WithMessage("Not enough Cards available.");
        }

        [Test]
        public void GenerateSet_12CardButOnly6Kingdom_ReturnNull()
        {
            //Arrange
            List<CardTypeToCard> kingdomTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };
            List<CardTypeToCard> nonKingdomTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } };

            DominionExpansion firstExpansion = new DominionExpansion();
            for (int i = 0; i < 5; i++)
            {
                firstExpansion.ContainedCards.Add(new Card() { CardTypeToCards = kingdomTypes });
            }

            DominionExpansion secondExpansion = new DominionExpansion();
            for (int i = 0; i < 5; i++)
            {
                secondExpansion.ContainedCards.Add(new Card() { CardTypeToCards = nonKingdomTypes });
            }

            List<DominionExpansion> expansions = new List<DominionExpansion> { firstExpansion, secondExpansion };


            Card oneCard = new Card { CardTypeToCards = new List<CardTypeToCard> { } };
            List<Card> cards = new List<Card> { oneCard };


            Action act = () => _testee.GenerateSet(expansions);

            //Act
            act.Should().Throw<Exception>().WithMessage("Not enough Cards available.");
        }

        [Test]
        public void GenerateSet_15AvailableCards_SetOf10Returned()
        {
            //Arrange
            List<CardTypeToCard> cardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                for (int i = 0; i < 5; i++)
                {
                    expansion.ContainedCards.Add(new Card() { CardTypeToCards = cardTypeToCards });
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
