using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardTypes;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponentsTest.StaticMethods
{
    [TestFixture]
    public class CardHelperTest
    {
        [Test]
        public void GetAdditionalCardsAlreadyIncluded_TwoAlreadyIncludedSomeOther_TwoReturned()
        {
            //Arrange
            CardArtifact artifactWithAlreadyIncluded = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = true } };

            Card card = new Card();
            card.CardArtifactsToCard = new List<CardArtifactToCard>
            {
                new CardArtifactToCard{ CardArtifact = TestDataDefines.CardArtifacts.YoungWitchCard},
                new CardArtifactToCard{ CardArtifact = TestDataDefines.CardArtifacts.ObeliskCard},
                new CardArtifactToCard{ CardArtifact = artifactWithAlreadyIncluded},
                new CardArtifactToCard{ CardArtifact = TestDataDefines.CardArtifacts.JourneyToken}
            };

            //Act
            IEnumerable<AdditionalCard> result = CardHelper.GetAdditionalCardsAlreadyIncluded(card, true);

            //Assert
            result.Should().HaveCount(2);
            result.Should().Contain(TestDataDefines.CardArtifacts.ObeliskCard.AdditionalCard);
            result.Should().Contain(artifactWithAlreadyIncluded.AdditionalCard);
        }

        [Test]
        public void IsNonSupplyType_IsSupplyType_False()
        {
            //Act
            bool result = CardHelper.IsNonSupplyType(TestDataDefines.Cards.Apothecary);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsNonSupplyType_IsNonSupplyType_True()
        {
            //Act
            bool result = CardHelper.IsNonSupplyType(TestDataDefines.Cards.Plan);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsNonSupplyType_MixedTypeTypes_True()
        {
            //Arrange
            Card card = new Card
            {
                CardTypeToCards = new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } ,
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            }
            };

            //Act
            bool result = CardHelper.IsNonSupplyType(card);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsSupplyType_IsSupplyType_True()
        {
            //Act
            bool result = CardHelper.IsSupplyType(TestDataDefines.Cards.Apothecary);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsSupplyType_IsNonSupplyType_False()
        {
            //Act
            bool result = CardHelper.IsSupplyType(TestDataDefines.Cards.Plan);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsSupplyType_MixedTypeTypes_True()
        {
            //Arrange
            Card card = new Card
            {
                CardTypeToCards = new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } ,
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            }
            };

            //Act
            bool result = CardHelper.IsSupplyType(card);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void GetSupplyCards_TwoSupplyOneMixedOneNonSupply_RetrieveSupplyCardsAndMixed3()
        {
            //Arrange
            List<CardTypeToCard> mixedTypes = new List<CardTypeToCard>() 
            { 
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } },
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            };
            List<Card> cards = new List<Card>();

            cards.Add(TestDataDefines.Cards.BridgeTroll);
            cards.Add(TestDataDefines.Cards.Apothecary);
            cards.Add(TestDataDefines.Cards.Plan);
            cards.Add(new Card { CardTypeToCards = mixedTypes });

            //Act
            List<Card> result = CardHelper.GetSupplyCards(cards);

            //Assert
            result.Should().HaveCount(3);
            result.Should().ContainEquivalentOf(TestDataDefines.Cards.BridgeTroll);
            result.Should().ContainEquivalentOf(TestDataDefines.Cards.Apothecary);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
        }

        [Test]
        public void GetNonSupplyCards_TwoSupplyOneMixedOneNonSupply_RetrieveNonSupplyCardsAndMixed3()
        {
            //Arrange
            List<CardTypeToCard> mixedTypes = new List<CardTypeToCard>() 
            { 
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } },
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            };
            List<Card> cards = new List<Card>();

            cards.Add(TestDataDefines.Cards.BridgeTroll);
            cards.Add(TestDataDefines.Cards.Apothecary);
            cards.Add(TestDataDefines.Cards.Plan);
            cards.Add(new Card { CardTypeToCards = mixedTypes });

            //Act
            List<Card> result = CardHelper.GetNonSupplyCards(cards);

            //Assert
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(TestDataDefines.Cards.Plan);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
        }

        [Test]
        public void GetArtifactsWithoutAdditional_GetOnlyArtifactsWithoutAdditional()
        {
            //Arrange
            List<Card> cards = new List<Card>
            {
                TestDataDefines.Cards.YoungWitch,
                TestDataDefines.Cards.BridgeTroll,
                TestDataDefines.Cards.Obelisk
            };

            //Act
            List<CardArtifact> result = CardHelper.GetArtifactsWithoutAdditional(cards).ToList();

            //Assert
            result.Should().HaveCount(1);
            result.Single().Should().BeEquivalentTo(TestDataDefines.CardArtifacts.MinusOneCoinMarker);
        }
    }
}
