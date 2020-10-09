using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.Probabilities;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
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
                    expansion.ContainedCards.Add(
                        new Card() { CardTypeToCards = supplyTypes, CardArtifactsToCard = new List<CardArtifactToCard>() });
                }

                expansions.Add(expansion);
            }

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());

            List<Card> nonSupplyCards = new List<Card>
            {
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}}, 
                    CardArtifactsToCard = new List<CardArtifactToCard>() },
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}}, 
                    CardArtifactsToCard = new List<CardArtifactToCard>() }
            };
            expansions[2].ContainedCards.AddRange(nonSupplyCards);

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            //Act
            GeneratedSetDto result = _testee.GenerateSet(expansions);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(10);
            result.NonSupplyCards.Should().HaveCount(1);
            result.NonSupplyCards.Should().Contain(nonSupplyCards.First());
            foreach(Card card in expansions[0].ContainedCards)
            {
                result.SupplyCardsWithoutAdditional.Should().Contain(card);
            }
            foreach(Card card in expansions[1].ContainedCards)
            {
                result.SupplyCardsWithoutAdditional.Should().Contain(card);
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
                    expansion.ContainedCards.Add(
                        new Card() { CardTypeToCards = cardTypeToCards, CardArtifactsToCard = new List<CardArtifactToCard>() });
                }

                expansions.Add(expansion);
            }

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());

            List<Card> nonSupplyCards = new List<Card>
            {
                new Card{ CardTypeToCards = new List<CardTypeToCard>{ new CardTypeToCard { CardType = new CardType {IsSupplyType = false}}}, 
                    CardArtifactsToCard = new List<CardArtifactToCard>() }
            };
            expansions[2].ContainedCards.AddRange(nonSupplyCards);

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            //Act
            GeneratedSetDto result = _testee.GenerateSet(expansions);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(10);
            result.NonSupplyCards.Should().Contain(nonSupplyCards.Single());
        }

        [Test]
        public void GenerateSet_1AdditionalCardWithAnother_TwoAdditionalReturned_NonSupplyTypeNotRespectedForAdditionalCard()
        {
            //Arrange
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(0);

            List<CardTypeToCard> cardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                for (int i = 0; i < 5; i++)
                {
                    expansion.ContainedCards.Add(
                        new Card() { CardTypeToCards = cardTypeToCards, Cost = new Cost(2, 0, false), CardArtifactsToCard = new List<CardArtifactToCard>() });
                }

                expansions.Add(expansion);
            }

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            allSupplyCards.First().CardArtifactsToCard = new List<CardArtifactToCard> 
            {
                new CardArtifactToCard { CardArtifact = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = false, MaxCost = 4, MinCost = 2 } } }
            };
            allSupplyCards[10].Cost = new Cost(5, 0, false);
            allSupplyCards[11].Cost = new Cost(1, 0, false);
            allSupplyCards[12].CardArtifactsToCard = new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = false, MaxCost = 4 } } }
            };

            expansions[2].ContainedCards.Add(new Card 
            { 
                CardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } } 
            });

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());
            List<Card> validCardsForFirstAdditional = allSupplyCards.Skip(12).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(validCardsForFirstAdditional, 1)).Returns(allSupplyCards.Skip(12).Take(1).ToList());
            List<Card> validCardsForSecondAdditional = allSupplyCards.Skip(11).ToList();
            validCardsForSecondAdditional.Remove(allSupplyCards[12]);
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(validCardsForSecondAdditional, 1)).Returns(allSupplyCards.Skip(11).Take(1).ToList());

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(list => list.Count == 1), 0)).Returns(new List<Card>());

            //Act
            GeneratedSetDto result = _testee.GenerateSet(expansions);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(10);
            result.GeneratedAdditionalCards.Should().HaveCount(2);
            result.GeneratedAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(allSupplyCards[12]);
            result.GeneratedAdditionalCards.Select(x => x.Parent).Should().Contain(allSupplyCards.First());
            result.GeneratedAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(allSupplyCards[11]);
            result.GeneratedAdditionalCards.Select(x => x.Parent).Should().Contain(allSupplyCards[12]);
        }

        [Test]
        public void GenerateSet_2AdditionalExistingCards_TwoAdditionalExistingReturned()
        {
            //Arrange
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(0);

            List<CardTypeToCard> cardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                for (int i = 0; i < 5; i++)
                {
                    expansion.ContainedCards.Add(
                        new Card() { CardTypeToCards = cardTypeToCards, Cost = new Cost(2, 0, false), CardArtifactsToCard = new List<CardArtifactToCard>() });
                }

                expansions.Add(expansion);
            }

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            allSupplyCards[0].CardArtifactsToCard = new List<CardArtifactToCard> 
            {
                new CardArtifactToCard { CardArtifact = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = true, MaxCost = 4, MinCost = 2 } } }
            };
            allSupplyCards[1].CardArtifactsToCard = new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = true, MaxCost = 4 } } }
            };

            List<Card> temporarySet = allSupplyCards.Take(10).ToList();

            Card firstParent = temporarySet[0];
            Card secondParent = temporarySet[1];
            Card firstAdditional = temporarySet.Skip(2).First();
            Card secondAdditional = temporarySet.Skip(3).First();

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(temporarySet);
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(y => y.Count == 9 && y.Contains(firstParent) == false), 1))
                .Returns(new List<Card> { firstAdditional });
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(y => y.Count == 8 && y.Contains(secondParent) == false), 1))
                .Returns(new List<Card> { secondAdditional });

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(new List<Card>(), 0)).Returns(new List<Card>());

            //Act
            GeneratedSetDto result = _testee.GenerateSet(expansions);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(8);
            result.GeneratedExistingAdditionalCards.Should().HaveCount(2);
            result.GeneratedExistingAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(firstAdditional);
            result.GeneratedExistingAdditionalCards.Select(x => x.Parent).Should().Contain(firstParent);
            result.GeneratedExistingAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(secondAdditional);
            result.GeneratedExistingAdditionalCards.Select(x => x.Parent).Should().Contain(secondParent);
        }
    }
}
