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
            CardArtifact artifactWithAlreadyIncluded1 = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = true } };
            CardArtifact artifactWithAlreadyIncluded2 = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = true } };
            CardArtifact artifactWithAdditional = new CardArtifact { AdditionalCard = new AdditionalCard { AlreadyIncludedCard = false } };
            CardArtifact artifactWithoutAdditional = new CardArtifact();

            Card card = new Card();
            card.CardArtifactsToCard = new List<CardArtifactToCard>
            {
                new CardArtifactToCard{ CardArtifact = artifactWithAdditional},
                new CardArtifactToCard{ CardArtifact = artifactWithAlreadyIncluded1},
                new CardArtifactToCard{ CardArtifact = artifactWithAlreadyIncluded2},
                new CardArtifactToCard{ CardArtifact = artifactWithoutAdditional}
            };

            //Act
            IEnumerable<AdditionalCard> result = CardHelper.GetAdditionalCardsAlreadyIncluded(card, true);

            //Assert
            result.Should().HaveCount(2);
            result.Should().Contain(artifactWithAlreadyIncluded1.AdditionalCard);
            result.Should().Contain(artifactWithAlreadyIncluded2.AdditionalCard);
        }

        [Test]
        public void IsNonSupplyType_IsSupplyType_False()
        {
            //Arrange
            Card card = new Card { CardTypeToCards = new List<CardTypeToCard> { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } } };

            //Act
            bool result = CardHelper.IsNonSupplyType(card);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsNonSupplyType_IsNonSupplyType_True()
        {
            //Arrange
            Card card = new Card { CardTypeToCards = new List<CardTypeToCard> { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } } };

            //Act
            bool result = CardHelper.IsNonSupplyType(card);

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
            //Arrange
            Card card = new Card { CardTypeToCards = new List<CardTypeToCard> { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } } };

            //Act
            bool result = CardHelper.IsSupplyType(card);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsSupplyType_IsNonSupplyType_False()
        {
            //Arrange
            Card card = new Card { CardTypeToCards = new List<CardTypeToCard> { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } } };

            //Act
            bool result = CardHelper.IsSupplyType(card);

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
            List<CardTypeToCard> supplyTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };
            List<CardTypeToCard> nonSupplyTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } };
            List<CardTypeToCard> mixedTypes = new List<CardTypeToCard>() 
            { 
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } },
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            };
            List<Card> cards = new List<Card>();

            cards.Add(new Card { CardTypeToCards = supplyTypes });
            cards.Add(new Card { CardTypeToCards = supplyTypes });
            cards.Add(new Card { CardTypeToCards = nonSupplyTypes });
            cards.Add(new Card { CardTypeToCards = mixedTypes });

            //Act
            List<Card> result = CardHelper.GetSupplyCards(cards);

            //Assert
            result.Should().HaveCount(3);
            result.Select(x => x.CardTypeToCards).Should().Contain(supplyTypes);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
            result.Select(x => x.CardTypeToCards).Should().NotContain(nonSupplyTypes);
        }

        [Test]
        public void GetNonSupplyCards_TwoSupplyOneMixedOneNonSupply_RetrieveNonSupplyCardsAndMixed3()
        {
            //Arrange
            List<CardTypeToCard> supplyTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = true } } };
            List<CardTypeToCard> nonSupplyTypes = new List<CardTypeToCard>() { new CardTypeToCard { CardType = new CardType { IsSupplyType = false } } };
            List<CardTypeToCard> mixedTypes = new List<CardTypeToCard>() 
            { 
                new CardTypeToCard { CardType = new CardType { IsSupplyType = false } },
                new CardTypeToCard { CardType = new CardType { IsSupplyType = true } }
            };
            List<Card> cards = new List<Card>();

            cards.Add(new Card { CardTypeToCards = supplyTypes });
            cards.Add(new Card { CardTypeToCards = supplyTypes });
            cards.Add(new Card { CardTypeToCards = nonSupplyTypes });
            cards.Add(new Card { CardTypeToCards = mixedTypes });

            //Act
            List<Card> result = CardHelper.GetNonSupplyCards(cards);

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.CardTypeToCards).Should().Contain(nonSupplyTypes);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
            result.Select(x => x.CardTypeToCards).Should().NotContain(supplyTypes);
        }
    }
}
