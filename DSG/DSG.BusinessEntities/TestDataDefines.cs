using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessEntities
{
    public static class TestDataDefines
    {
        public static class Costs
        {
            public static Cost Three => new Cost(3, 0, false);
            public static Cost Four => new Cost(4, 0, false);
            public static Cost Five => new Cost(5, 0, false);
            public static Cost TwoMoneyPotion => new Cost(2, 0, true);
            public static Cost TwoMoneyFourDept => new Cost(2, 4, false);
        }

        public static class CardTypes
        {
            public static CardType Action => new CardType { Name = "Action", Id = CardTypeEnum.Action, IsSupplyType = true };
            public static CardType Treasure => new CardType { Name = "Victory", Id = CardTypeEnum.Treasure, IsSupplyType = true };
            public static CardType Victory => new CardType { Name = "Victory", Id = CardTypeEnum.Victory, IsSupplyType = true };
            public static CardType Event => new CardType { Name = "Event", Id = CardTypeEnum.Event, IsSupplyType = false };
            public static CardType Landmark => new CardType { Name = "Event", Id = CardTypeEnum.Landmark, IsSupplyType = false };
        }

        public static class CardTypeToCards
        {
            public static List<CardTypeToCard> ActionType => new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = CardTypes.Action }
            };
            public static List<CardTypeToCard> TreasureType => new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = CardTypes.Treasure }
            };
            public static List<CardTypeToCard> EventType => new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = CardTypes.Event }
            };
            public static List<CardTypeToCard> LandmarkType => new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = CardTypes.Landmark }
            };
            public static List<CardTypeToCard> ActionAndVictory => new List<CardTypeToCard>
            {
                new CardTypeToCard { CardType = CardTypes.Action },
                new CardTypeToCard { CardType = CardTypes.Victory }
            };
        }

        public static class DominionExpansions
        {
            public static DominionExpansion Adventure => new DominionExpansion { Name = "Adventure" };
            public static DominionExpansion Cornucopia => new DominionExpansion { Name = "Cornucopia" };
            public static DominionExpansion Empires => new DominionExpansion { Name = "Empires" };
            public static DominionExpansion Alchemy => new DominionExpansion { Name = "Alchemy" };
        }

        public static class AdditionalCards
        {
            public static AdditionalCard Additional => new AdditionalCard { AlreadyIncludedCard = false, MaxCost = 4, MinCost = 2 };
            public static AdditionalCard Existing => new AdditionalCard { AlreadyIncludedCard = true, MinCost = null, MaxCost = 5 };
        }

        public static class CardArtifacts
        {
            public static CardArtifact YoungWitchCard => new CardArtifact 
            {
                Name = "Young Witch Card", DominionExpansion = DominionExpansions.Cornucopia, AdditionalCard = AdditionalCards.Additional  
            };
            public static CardArtifact MinusOneCoinMarker => new CardArtifact 
            { 
                Name = "-1 Coin Marker", DominionExpansion = DominionExpansions.Adventure 
            };
            public static CardArtifact MinusOneCardMarker => new CardArtifact 
            { 
                Name = "-1 Card Marker", DominionExpansion = DominionExpansions.Adventure 
            };
            public static CardArtifact JourneyToken => new CardArtifact 
            { 
                Name = "Journey Token", DominionExpansion = DominionExpansions.Adventure 
            };
            public static CardArtifact TrashToken => new CardArtifact 
            { 
                Name = "Trash Token", DominionExpansion = DominionExpansions.Adventure 
            };
            public static CardArtifact ObeliskCard => new CardArtifact 
            { 
                Name = "Obelisk Card", DominionExpansion = DominionExpansions.Empires, AdditionalCard = AdditionalCards.Existing 
            };
        }

        public static class CardArtifactToCards
        {
            public static List<CardArtifactToCard> YoungWitchArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.YoungWitchCard }
            };
            public static List<CardArtifactToCard> MinusOneCoinArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.MinusOneCoinMarker }
            };
            public static List<CardArtifactToCard> MinusOneCardArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.MinusOneCardMarker }
            };
            public static List<CardArtifactToCard> JourneyTokenArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.JourneyToken }
            };
            public static List<CardArtifactToCard> TrashToken => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.TrashToken }
            };
            public static List<CardArtifactToCard> ObelistArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.ObeliskCard }
            };
        }

        public static class Cards
        {
            public static Card YoungWitch => new Card
            {
                Name = "Young Witch",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardArtifactsToCard = CardArtifactToCards.YoungWitchArtifact,
                CardTypeToCards = CardTypeToCards.ActionType,
                Cost = Costs.Four
            };

            public static Card BridgeTroll => new Card
            {
                Name = "Bridge Troll",
                DominionExpansion = DominionExpansions.Adventure,
                CardArtifactsToCard = CardArtifactToCards.MinusOneCoinArtifact,
                CardTypeToCards = CardTypeToCards.ActionType,
                Cost = Costs.Five
            };

            public static Card Relic => new Card
            {
                Name = "Relic",
                DominionExpansion = DominionExpansions.Adventure,
                CardArtifactsToCard = CardArtifactToCards.MinusOneCardArtifact,
                CardTypeToCards = CardTypeToCards.TreasureType,
                Cost = Costs.Five
            };

            public static Card Ranger => new Card
            {
                Name = "Ranger",
                DominionExpansion = DominionExpansions.Adventure,
                CardArtifactsToCard = CardArtifactToCards.JourneyTokenArtifact,
                CardTypeToCards = CardTypeToCards.ActionType,
                Cost = Costs.Four
            };

            public static Card Plan => new Card
            {
                Name = "Plan",
                DominionExpansion = DominionExpansions.Adventure,
                CardArtifactsToCard = CardArtifactToCards.JourneyTokenArtifact,
                CardTypeToCards = CardTypeToCards.EventType,
                Cost = Costs.Three
            };

            public static Card Mill => new Card
            {
                Name = "Mill",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionAndVictory,
                Cost = Costs.Four
            };

            public static Card Apothecary => new Card
            {
                Name = "Apothecary",
                DominionExpansion = DominionExpansions.Alchemy,
                CardTypeToCards = CardTypeToCards.ActionType,
                Cost = Costs.TwoMoneyPotion
            };
        }

        public static class GeneratedAdditionalCards
        {
            public static GeneratedAdditionalCard Relic => new GeneratedAdditionalCard(Cards.Relic, Cards.YoungWitch);
            public static GeneratedAdditionalCard Ranger => new GeneratedAdditionalCard(Cards.Ranger, Cards.YoungWitch);
        }
    }
}
