using DSG.BusinessEntities.CardArtifacts;
using DSG.BusinessEntities.CardTypes;
using DSG.BusinessEntities.GenerationProfiles;
using System.Collections.Generic;

namespace DSG.BusinessEntities
{
    public static class TestDataDefines
    {
        #region Cost

        public static class Costs
        {
            public static Cost One => new Cost(1, 0, false);
            public static Cost Three => new Cost(3, 0, false);
            public static Cost Four => new Cost(4, 0, false);
            public static Cost Five => new Cost(5, 0, false);
            public static Cost TwoMoneyPotion => new Cost(2, 0, true);
            public static Cost TwoMoneyFourDept => new Cost(2, 4, false);
        }

        #endregion


        #region CardTypes

        public static class CardTypes
        {
            public static CardType Action => new CardType { Name = "Action", Id = CardTypeEnum.Action, IsSupplyType = true };
            public static CardType Treasure => new CardType { Name = "Victory", Id = CardTypeEnum.Treasure, IsSupplyType = true };
            public static CardType Victory => new CardType { Name = "Victory", Id = CardTypeEnum.Victory, IsSupplyType = true };
            public static CardType Event => new CardType { Name = "Event", Id = CardTypeEnum.Event, IsSupplyType = false };
            public static CardType Landmark => new CardType { Name = "Event", Id = CardTypeEnum.Landmark, IsSupplyType = false };
        }

        #endregion


        #region CardTypeToCards

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

        #endregion


        #region DominionExpansions

        public static class DominionExpansions
        {
            public static DominionExpansion Adventure => new DominionExpansion { Name = "Adventure" };
            public static DominionExpansion Cornucopia => new DominionExpansion { Name = "Cornucopia" };
            public static DominionExpansion Empires => new DominionExpansion { Name = "Empires" };
            public static DominionExpansion Alchemy => new DominionExpansion { Name = "Alchemy" };
        }

        #endregion


        #region AdditionalCards

        public static class AdditionalCards
        {
            public static AdditionalCard AdditionalMinMax => new AdditionalCard { AlreadyIncludedCard = false, MaxCost = 4, MinCost = 2 };
            public static AdditionalCard AdditionalMin => new AdditionalCard { AlreadyIncludedCard = false, MinCost = 2, MaxCost = null };
            public static AdditionalCard ExistingMax => new AdditionalCard { AlreadyIncludedCard = true, MinCost = null, MaxCost = 4 };
            public static AdditionalCard ExistingMinMax => new AdditionalCard { AlreadyIncludedCard = true, MinCost = 2, MaxCost = 4 };
        }

        #endregion


        #region CardArtifacts

        public static class CardArtifacts
        {
            public static CardArtifact YoungWitchCard => new CardArtifact 
            {
                Name = "Young Witch Card", DominionExpansion = DominionExpansions.Cornucopia, AdditionalCard = AdditionalCards.AdditionalMinMax, AmountOfAdditionalCards = 1
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
                Name = "Obelisk Card", DominionExpansion = DominionExpansions.Empires, AdditionalCard = AdditionalCards.ExistingMax, AmountOfAdditionalCards = 1 
            };
            public static CardArtifact Prices => new CardArtifact 
            { 
                Name = "Prices", DominionExpansion = DominionExpansions.Cornucopia
            };
            public static CardArtifact TestAdditionalArtifact => new CardArtifact 
            { 
                Name = "Test Additional", DominionExpansion = DominionExpansions.Cornucopia, AdditionalCard = AdditionalCards.AdditionalMin, AmountOfAdditionalCards = 1
            };
            public static CardArtifact TestExistingArtifact => new CardArtifact 
            { 
                Name = "Test Existing", DominionExpansion = DominionExpansions.Cornucopia, AdditionalCard = AdditionalCards.ExistingMinMax, AmountOfAdditionalCards = 1
            };
        }

        #endregion


        #region CardArtifactsToCards

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
            public static List<CardArtifactToCard> TrashTokenArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.TrashToken }
            };
            public static List<CardArtifactToCard> ObelistArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.ObeliskCard }
            };
            public static List<CardArtifactToCard> PricesArtifact => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.Prices }
            };
            public static List<CardArtifactToCard> TestAdditional => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.TestAdditionalArtifact }
            };
            public static List<CardArtifactToCard> TestExisting => new List<CardArtifactToCard>
            {
                new CardArtifactToCard { CardArtifact = CardArtifacts.TestExistingArtifact }
            };

            public static List<CardArtifactToCard> EmptyList => new List<CardArtifactToCard>();
        }

        #endregion


        #region Cards

        public static class Cards
        {
            #region Cornucopia

            public static Card YoungWitch => new Card
            {
                Name = "Young Witch",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardArtifactsToCard = CardArtifactToCards.YoungWitchArtifact,
                CardTypeToCards = CardTypeToCards.ActionType,
                Cost = Costs.Four
            };
            
            public static Card Menagerie => new Card
            {
                Name = "Menagerie",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Three
            };
            
            public static Card Tournament => new Card
            {
                Name = "Tournament",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.PricesArtifact,
                Cost = Costs.Four
            };
            
            public static Card HorseTraders => new Card
            {
                Name = "Horse Traders",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Four
            };
            
            public static Card Remake => new Card
            {
                Name = "Remake",
                DominionExpansion = DominionExpansions.Cornucopia,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Four
            };

            #endregion

            #region Adventure

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

            public static Card Dungeon => new Card
            {
                Name = "Dungeon",
                DominionExpansion = DominionExpansions.Adventure,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Three
            };

            public static Card Magpie => new Card
            {
                Name = "Magpie",
                DominionExpansion = DominionExpansions.Adventure,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Four
            };

            public static Card Plan => new Card
            {
                Name = "Plan",
                DominionExpansion = DominionExpansions.Adventure,
                CardArtifactsToCard = CardArtifactToCards.TrashTokenArtifact,
                CardTypeToCards = CardTypeToCards.EventType,
                Cost = Costs.Three
            };

            #endregion

            public static Card Mill => new Card
            {
                Name = "Mill",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionAndVictory,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Four
            };

            public static Card Apothecary => new Card
            {
                Name = "Apothecary",
                DominionExpansion = DominionExpansions.Alchemy,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.TwoMoneyPotion
            };

            public static Card PoorHouse => new Card
            {
                Name = "Poor House",
                DominionExpansion = DominionExpansions.Alchemy,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.One
            };

            public static Card Apprentice => new Card
            {
                Name = "Apprentice",
                DominionExpansion = DominionExpansions.Alchemy,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Five
            };

            public static Card Enchantress => new Card
            {
                Name = "Apprentice",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.EmptyList,
                Cost = Costs.Three
            };

            public static Card TestWithAdditional => new Card
            {
                Name = "Test With Additional",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.TestAdditional,
                Cost = Costs.Three
            };

            public static Card TestWithExisting => new Card
            {
                Name = "Test With Existing",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.TestExisting,
                Cost = Costs.Three
            };

            public static Card TestWithExistingOnlyMaxCost => new Card
            {
                Name = "Test With Existing Only Max Cost",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.ActionType,
                CardArtifactsToCard = CardArtifactToCards.ObelistArtifact,
                Cost = Costs.Three
            };

            public static Card Obelisk => new Card
            {
                Name = "Obelisk",
                DominionExpansion = DominionExpansions.Empires,
                CardTypeToCards = CardTypeToCards.LandmarkType,
                CardArtifactsToCard = CardArtifactToCards.ObelistArtifact
            };
        }

        #endregion


        #region GeneratedAdditionalCards

        public static class GeneratedAdditionalCards
        {
            public static GeneratedAdditionalCard Relic => new GeneratedAdditionalCard(Cards.Relic, Cards.YoungWitch);
            public static GeneratedAdditionalCard Ranger => new GeneratedAdditionalCard(Cards.Ranger, Cards.YoungWitch);
        }

        #endregion


        #region PropabilitiesForNonSupplyCards

        public static class PropabilitiesForNonSupplyCards
        {
            public static PropabilityForNonSupplyCards Zero = new PropabilityForNonSupplyCards(0, 0, 0, 0);

            public static PropabilityForNonSupplyCards Default = new PropabilityForNonSupplyCards(50, 30, 7, 0);
        }

        #endregion
    }
}
