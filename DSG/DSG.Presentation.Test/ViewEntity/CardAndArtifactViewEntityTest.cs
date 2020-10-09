using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardTypes;
using DSG.Presentation.ViewEntity;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DSG.Presentation.Test.ViewEntity
{
    [TestFixture]
    public class CardAndArtifactViewEntityTest
    {
        private CardAndArtifactViewEntity _testee;

        [Test]
        public void CardAndArtifactViewEntity_InsertSupplyCard()
        {
            //Arrange
            Card card = new Card
            {
                Name = "TestCard",
                Cost = new Cost(4, 2, true),
                CardTypeToCards = new List<CardTypeToCard> 
                { 
                    new CardTypeToCard { CardType = new CardType { Name = "FirstType", IsSupplyType = true} },
                    new CardTypeToCard { CardType = new CardType { Name = "SecondType", IsSupplyType = true} } 
                },
                DominionExpansion = new DominionExpansion { Name = "Expansion"}
            };

            //Act
            _testee = new CardAndArtifactViewEntity(card);

            //Assert
            _testee.Name.Should().Be("TestCard");
            _testee.Cost.Should().Be("6+T");
            _testee.Type.Should().Be("FirstType, SecondType");
            _testee.Set.Should().Be("Expansion");
            _testee.BelongsTo.Should().BeNull();
        }

        [Test]
        public void CardAndArtifactViewEntity_InsertNonSupplyCard()
        {
            //Arrange
            Card card = new Card
            {
                Name = "TestCard",
                Cost = new Cost(4, 2, true),
                CardTypeToCards = new List<CardTypeToCard> 
                { 
                    new CardTypeToCard { CardType = new CardType { Name = "FirstType", IsSupplyType = false} },
                    new CardTypeToCard { CardType = new CardType { Name = "SecondType", IsSupplyType = false} } 
                },
                DominionExpansion = new DominionExpansion { Name = "Expansion"}
            };

            //Act
            _testee = new CardAndArtifactViewEntity(card);

            //Assert
            _testee.Name.Should().Be("TestCard");
            _testee.Cost.Should().BeNull();
            _testee.Type.Should().Be("FirstType, SecondType");
            _testee.Set.Should().Be("Expansion");
            _testee.BelongsTo.Should().BeNull();
        }

        [Test]
        public void CardAndArtifactViewEntity_InsertAdditionalSupplyCard()
        {
            //Arrange
            Card generatedAdditional = new Card
            {
                Name = "TestCard",
                Cost = new Cost(4, 2, true),
                CardTypeToCards = new List<CardTypeToCard> 
                { 
                    new CardTypeToCard { CardType = new CardType { Name = "FirstType", IsSupplyType = true} },
                    new CardTypeToCard { CardType = new CardType { Name = "SecondType", IsSupplyType = true} } 
                },
                DominionExpansion = new DominionExpansion { Name = "Expansion"}
            };

            Card parent = new Card { Name = "Parent" };

            GeneratedAdditionalCard generatedAdditionalCard = new GeneratedAdditionalCard(generatedAdditional, parent);

            //Act
            _testee = new CardAndArtifactViewEntity(generatedAdditionalCard);

            //Assert
            _testee.Name.Should().Be("TestCard");
            _testee.Cost.Should().Be("6+T");
            _testee.Type.Should().Be("FirstType, SecondType");
            _testee.Set.Should().Be("Expansion");
            _testee.BelongsTo.Should().Be("Parent");
        }

        [Test]
        public void CardAndArtifactViewEntity_InsertArtifact()
        {
            //Arrange
            CardArtifact artifact = new CardArtifact { Name = "Artifact", DominionExpansion = new DominionExpansion { Name = "Expansion" } };

            //Act
            _testee = new CardAndArtifactViewEntity(artifact);

            //Assert
            _testee.Name.Should().Be("Artifact");
            _testee.Cost.Should().BeNull();
            _testee.Type.Should().Be("Artifact");
            _testee.Set.Should().Be("Expansion");
            _testee.BelongsTo.Should().BeNull();
        }
    }
}
