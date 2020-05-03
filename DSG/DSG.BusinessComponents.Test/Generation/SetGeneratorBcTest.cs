using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.Probabilities;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
using DSG.Common.Exceptions;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DSG.BusinessComponentsTest.Generation
{
    [TestFixture]
    public class SetGeneratorBcTest
    {
        private SetGeneratorBc _testee;
        private Mock<IGetIntByProbabilityBc> _getIntByProbabilityBcMock;
        private Mock<IShuffleListBc<Card>> _shuffleListBcMock;

        [SetUp]
        public void Setup()
        {
            _testee = new SetGeneratorBc();

            _getIntByProbabilityBcMock = new Mock<IGetIntByProbabilityBc>();
            _testee.GetIntByProbabilityBc = _getIntByProbabilityBcMock.Object;

            _shuffleListBcMock = new Mock<IShuffleListBc<Card>>();
            _testee.ShuffleListBc = _shuffleListBcMock.Object;
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
            act.Should().Throw<NotEnoughCardsAvailableException>().WithMessage("Not enough Cards available.");
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
            act.Should().Throw<NotEnoughCardsAvailableException>().WithMessage("Not enough Cards available.");
        }

        [Test]
        public void GenerateSet_15AvailableSupplyCards2NonSupply_SetOf10Plus1Returned()
        {
            //Arrange
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(1);

            List<CardTypeToCard> supplyTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                for (int i = 0; i < 5; i++)
                {
                    expansion.ContainedCards.Add(new Card() { CardTypeToCards = supplyTypes });
                }

                expansions.Add(expansion);
            }

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());

            List<Card> nonSupplyCards = new List<Card>
            {
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}} },
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}} }
            };
            expansions[2].ContainedCards.AddRange(nonSupplyCards);

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            //Act
            List<Card> result = _testee.GenerateSet(expansions);

            //Assert
            result.Should().HaveCount(11);
            result.Should().Contain(nonSupplyCards.First());
            foreach(Card card in expansions[0].ContainedCards)
            {
                result.Should().Contain(card);
            }
            foreach(Card card in expansions[1].ContainedCards)
            {
                result.Should().Contain(card);
            }
        }

        [Test]
        public void GenerateSet_1NonSupplyCardBut2Chosen_DoNotThrowReturnOnlyOne()
        {
            //Arrange
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(2);

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

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());

            List<Card> nonSupplyCards = new List<Card>
            {
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}} }
            };
            expansions[2].ContainedCards.AddRange(nonSupplyCards);

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            //Act
            List<Card> result = _testee.GenerateSet(expansions);

            //Assert
            result.Should().HaveCount(11);
            result.Should().Contain(nonSupplyCards.Single());
        }
    }
}
