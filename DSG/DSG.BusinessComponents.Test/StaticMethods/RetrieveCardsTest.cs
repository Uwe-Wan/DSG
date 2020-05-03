using DSG.BusinessComponents.StaticMethods;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardTypes;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessComponentsTest.StaticMethods
{
    [TestFixture]
    public class RetrieveCardsTest
    {
        [Test]
        public void SupplyOrOthers_TwoSupplyOneMixedOneNonSupply_RetrieveSupplyCardsAndMixed3()
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
            List<Card> result = RetrieveCards.SupplyOrOthers(cards, true);

            //Assert
            result.Should().HaveCount(3);
            result.Select(x => x.CardTypeToCards).Should().Contain(supplyTypes);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
            result.Select(x => x.CardTypeToCards).Should().NotContain(nonSupplyTypes);
        }

        [Test]
        public void SupplyOrOthers_TwoSupplyOneMixedOneNonSupply_RetrieveNonSupplyCardsAndMixed3()
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
            List<Card> result = RetrieveCards.SupplyOrOthers(cards, false);

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.CardTypeToCards).Should().Contain(nonSupplyTypes);
            result.Select(x => x.CardTypeToCards).Should().Contain(mixedTypes);
            result.Select(x => x.CardTypeToCards).Should().NotContain(supplyTypes);
        }
    }
}
