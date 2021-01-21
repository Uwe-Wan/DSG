using System;
using System.Collections.Generic;
using System.Linq;
using DSG.BusinessComponents.Generation;
using DSG.BusinessComponents.Probabilities;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardTypes;
using DSG.Common.Exceptions;
using DSG.Common.Provider;
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
        private Mock<IRandomProvider> _randomMock;

        [SetUp]
        public void Setup()
        {
            _testee = new SetGeneratorBc();

            _getIntByProbabilityBcMock = new Mock<IGetIntByProbabilityBc>();
            _testee.GetIntByProbabilityBc = _getIntByProbabilityBcMock.Object;

            _shuffleListBcMock = new Mock<IShuffleListBc<Card>>();
            _testee.ShuffleListBc = _shuffleListBcMock.Object;

            _randomMock = new Mock<IRandomProvider>();
            _testee.RandomProvider = _randomMock.Object;
        }

        [Test]
        public void GenerateSet_LessThan10AvailableCards_ReturnNull()
        {
            //Arrange
            Card oneCard = new Card { CardTypeToCards = new List<CardTypeToCard> { } };
            List<Card> cards = new List<Card> { oneCard };

            DominionExpansion expansion = new DominionExpansion { ContainedCards = cards };
            List<DominionExpansion> expansions = new List<DominionExpansion> { expansion };

            GenerationParameterDto generationParameter = new GenerationParameterDto(new List<IsDominionExpansionSelectedDto>(), 0, 0, new Dictionary<int, int>());

            Action act = () => _testee.GenerateSet(generationParameter);

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

            GenerationParameterDto generationParameter = new GenerationParameterDto(new List<IsDominionExpansionSelectedDto>(), 0, 0, new Dictionary<int, int>());

            Action act = () => _testee.GenerateSet(generationParameter);

            //Act
            act.Should().Throw<NotEnoughCardsAvailableException>().WithMessage("Not enough Cards available.");
        }

        [Test]
        public void GenerateSet_15AvailableSupplyCards2NonSupply_SetOf10Plus1Returned_PlatinumColonyAndSheltersDrawn()
        {
            //Arrange
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 50);
            propabilitiesForNonSuppliesByAmount.Add(2, 30);
            propabilitiesForNonSuppliesByAmount.Add(3, 7);
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(1);

            //this is to draw platinum, colony and shelters
            Queue<int> propabilities = new Queue<int>();
            propabilities.Enqueue(19);
            propabilities.Enqueue(9);
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(propabilities.Dequeue);


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
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            GenerationParameterDto generationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 20, 10, propabilitiesForNonSuppliesByAmount);

            //Act
            GeneratedSetDto result = _testee.GenerateSet(generationParameter);

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

            result.HasPlatinumAndColony.Should().BeTrue();
            result.HasShelters.Should().BeTrue();
        }

        [Test]
        public void GenerateSet_1NonSupplyCardBut2Chosen_DoNotThrowReturnOnlyOne()
        {
            //Arrange
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 50);
            propabilitiesForNonSuppliesByAmount.Add(2, 30);
            propabilitiesForNonSuppliesByAmount.Add(3, 7);
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(2);

            //this is to NOT draw platinum, colony and shelters, take the minimal values that should not lead into those being selected
            Queue<int> propabilities = new Queue<int>();
            propabilities.Enqueue(20);
            propabilities.Enqueue(10);
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(propabilities.Dequeue);

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
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(nonSupplyCards, 1)).Returns(nonSupplyCards.Take(1).ToList());

            GenerationParameterDto generationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 20, 10, propabilitiesForNonSuppliesByAmount);

            //Act
            GeneratedSetDto result = _testee.GenerateSet(generationParameter);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(10);
            result.NonSupplyCards.Should().Contain(nonSupplyCards.Single());

            result.HasPlatinumAndColony.Should().BeFalse();
            result.HasShelters.Should().BeFalse();
        }

        [Test]
        public void GenerateSet_1AdditionalCardWithAnother_TwoAdditionalReturned_NonSupplyTypeNotRespectedForAdditionalCard()
        {
            //Arrange
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 50);
            propabilitiesForNonSuppliesByAmount.Add(2, 30);
            propabilitiesForNonSuppliesByAmount.Add(3, 7);
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(0);

            //this is to NOT draw platinum, colony and shelters, take the minimal values that should not lead into those being selected
            Queue<int> propabilities = new Queue<int>();
            propabilities.Enqueue(20);
            propabilities.Enqueue(10);
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(propabilities.Dequeue);

            List<CardTypeToCard> cardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 3; j++)
            {
                DominionExpansion expansion = new DominionExpansion();
                expansions.Add(expansion);
            }

            expansions[0].ContainedCards.Add(TestDataDefines.Cards.YoungWitch);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.Tournament);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.Menagerie);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.Remake);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.HorseTraders);

            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Dungeon);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.BridgeTroll);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Magpie);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Ranger);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Relic);

            expansions[2].ContainedCards.Add(TestDataDefines.Cards.PoorHouse);
            expansions[2].ContainedCards.Add(TestDataDefines.Cards.Apprentice);
            expansions[2].ContainedCards.Add(TestDataDefines.Cards.TestWithAdditional);
            expansions[2].ContainedCards.Add(TestDataDefines.Cards.Dungeon);
            expansions[2].ContainedCards.Add(TestDataDefines.Cards.Apothecary);

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();
            expansions[2].ContainedCards.Add(TestDataDefines.Cards.Plan);
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(allSupplyCards.Take(10).ToList());
            List<Card> validCardsForFirstAdditional = allSupplyCards.Skip(12).ToList();
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(validCardsForFirstAdditional, 1)).Returns(allSupplyCards.Skip(12).Take(1).ToList());
            List<Card> validCardsForSecondAdditional = allSupplyCards.Skip(11).ToList();
            validCardsForSecondAdditional.Remove(allSupplyCards[12]);
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(validCardsForSecondAdditional, 1)).Returns(allSupplyCards.Skip(11).Take(1).ToList());

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(list => list.Count == 1), 0)).Returns(new List<Card>());

            GenerationParameterDto generationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 20, 10, propabilitiesForNonSuppliesByAmount);

            //Act
            GeneratedSetDto result = _testee.GenerateSet(generationParameter);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(10);
            result.GeneratedAdditionalCards.Should().HaveCount(2);
            result.GeneratedAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(allSupplyCards[12]);
            result.GeneratedAdditionalCards.Select(x => x.Parent).Should().Contain(allSupplyCards.First());
            result.GeneratedAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(allSupplyCards[11]);
            result.GeneratedAdditionalCards.Select(x => x.Parent).Should().Contain(allSupplyCards[12]);

            result.HasPlatinumAndColony.Should().BeFalse();
            result.HasShelters.Should().BeFalse();
        }

        [Test]
        public void GenerateSet_2AdditionalExistingCards_TwoAdditionalExistingReturned()
        {
            //Arrange
            Dictionary<int, int> propabilitiesForNonSuppliesByAmount = new Dictionary<int, int>();
            propabilitiesForNonSuppliesByAmount.Add(1, 50);
            propabilitiesForNonSuppliesByAmount.Add(2, 30);
            propabilitiesForNonSuppliesByAmount.Add(3, 7);
            _getIntByProbabilityBcMock.Setup(x => x.GetRandomIntInBetweenZeroAndInputParameterCount(50, 30, 7)).Returns(0);

            //this is to NOT draw platinum, colony and shelters, take the minimal values that should not lead into those being selected
            Queue<int> propabilities = new Queue<int>();
            propabilities.Enqueue(20);
            propabilities.Enqueue(10);
            _randomMock.Setup(x => x.GetRandomIntegerByUpperBoarder(100)).Returns(propabilities.Dequeue);

            List<CardTypeToCard> cardTypeToCards = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };

            List<DominionExpansion> expansions = new List<DominionExpansion>();
            for (int j = 0; j < 2; j++)
            {
                DominionExpansion expansion = new DominionExpansion();

                expansions.Add(expansion);
            }

            expansions[0].ContainedCards.Add(TestDataDefines.Cards.TestWithExisting);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.TestWithExistingOnlyMaxCost);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.Menagerie);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.Remake);
            expansions[0].ContainedCards.Add(TestDataDefines.Cards.HorseTraders);

            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Dungeon);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.BridgeTroll);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Magpie);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Ranger);
            expansions[1].ContainedCards.Add(TestDataDefines.Cards.Relic);
            List<IsDominionExpansionSelectedDto> isDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();

            List<Card> allSupplyCards = expansions.SelectMany(x => x.ContainedCards).ToList();

            List<Card> temporarySet = allSupplyCards.ToList();

            Card firstParent = temporarySet[0];
            Card secondParent = temporarySet[1];
            Card firstAdditional = temporarySet.Skip(2).First();
            Card secondAdditional = temporarySet.Skip(3).First();

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(allSupplyCards, 10)).Returns(temporarySet);
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(y => y.Count == 7 && y.Contains(firstParent) == false), 1))
                .Returns(new List<Card> { firstAdditional });
            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(It.Is<List<Card>>(y => y.Count == 6 && y.Contains(secondParent) == false), 1))
                .Returns(new List<Card> { secondAdditional });

            _shuffleListBcMock.Setup(x => x.ReturnGivenNumberOfRandomElementsFromList(new List<Card>(), 0)).Returns(new List<Card>());

            GenerationParameterDto generationParameter = new GenerationParameterDto(isDominionExpansionSelectedDtos, 20, 10, propabilitiesForNonSuppliesByAmount);

            //Act
            GeneratedSetDto result = _testee.GenerateSet(generationParameter);

            //Assert
            result.SupplyCardsWithoutAdditional.Should().HaveCount(8);
            result.GeneratedExistingAdditionalCards.Should().HaveCount(2);
            result.GeneratedExistingAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(firstAdditional);
            result.GeneratedExistingAdditionalCards.Select(x => x.Parent).Should().Contain(firstParent);
            result.GeneratedExistingAdditionalCards.Select(x => x.AdditionalCard).Should().Contain(secondAdditional);
            result.GeneratedExistingAdditionalCards.Select(x => x.Parent).Should().Contain(secondParent);

            result.HasPlatinumAndColony.Should().BeFalse();
            result.HasShelters.Should().BeFalse();
        }
    }
}
