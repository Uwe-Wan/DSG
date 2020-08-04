namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlreadyIncludedCard = c.Boolean(nullable: false),
                        MaxCosts = c.Int(),
                        MinCosts = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Card",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CostId = c.Int(nullable: false),
                        DominionExpansionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cost", t => t.CostId, cascadeDelete: true)
                .ForeignKey("dbo.DominionExpansion", t => t.DominionExpansionId, cascadeDelete: true)
                .Index(t => new { t.Name, t.DominionExpansionId }, unique: true, name: "UQX_Card_Name_DominionExpansionId")
                .Index(t => t.CostId);
            
            CreateTable(
                "dbo.CardArtifacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DominionExpansionId = c.Int(),
                        AdditionalCardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalCards", t => t.AdditionalCardId)
                .ForeignKey("dbo.DominionExpansion", t => t.DominionExpansionId)
                .Index(t => new { t.Name, t.DominionExpansionId }, unique: true, name: "UQX_CardAttribute_Name_DominionExpansionId")
                .Index(t => t.AdditionalCardId);
            
            CreateTable(
                "dbo.DominionExpansion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.CardSubTypeToCards",
                c => new
                    {
                        CardId = c.Int(nullable: false),
                        CardSubTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardId, t.CardSubTypeId })
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: true)
                .ForeignKey("dbo.CardSubTypes", t => t.CardSubTypeId, cascadeDelete: true)
                .Index(t => t.CardId)
                .Index(t => t.CardSubTypeId);
            
            CreateTable(
                "dbo.CardSubTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CardTypeToCards",
                c => new
                    {
                        CardId = c.Int(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardId, t.CardTypeId })
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: true)
                .ForeignKey("dbo.CardType", t => t.CardTypeId, cascadeDelete: true)
                .Index(t => t.CardId)
                .Index(t => t.CardTypeId);
            
            CreateTable(
                "dbo.CardType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsSupplyType = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Money = c.Int(nullable: false),
                        Potion = c.Boolean(nullable: false),
                        Dept = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Money, t.Potion, t.Dept }, unique: true, name: "UQX_Cost_Money_Dept_Potion");
            
            CreateTable(
                "dbo.CardArtifactCards",
                c => new
                    {
                        CardArtifact_Id = c.Int(nullable: false),
                        Card_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardArtifact_Id, t.Card_Id })
                .ForeignKey("dbo.CardArtifacts", t => t.CardArtifact_Id, cascadeDelete: true)
                .ForeignKey("dbo.Card", t => t.Card_Id, cascadeDelete: true)
                .Index(t => t.CardArtifact_Id)
                .Index(t => t.Card_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Card", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.Card", "CostId", "dbo.Cost");
            DropForeignKey("dbo.CardTypeToCards", "CardTypeId", "dbo.CardType");
            DropForeignKey("dbo.CardTypeToCards", "CardId", "dbo.Card");
            DropForeignKey("dbo.CardSubTypeToCards", "CardSubTypeId", "dbo.CardSubTypes");
            DropForeignKey("dbo.CardSubTypeToCards", "CardId", "dbo.Card");
            DropForeignKey("dbo.CardArtifacts", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.CardArtifactCards", "Card_Id", "dbo.Card");
            DropForeignKey("dbo.CardArtifactCards", "CardArtifact_Id", "dbo.CardArtifacts");
            DropForeignKey("dbo.CardArtifacts", "AdditionalCardId", "dbo.AdditionalCards");
            DropIndex("dbo.CardArtifactCards", new[] { "Card_Id" });
            DropIndex("dbo.CardArtifactCards", new[] { "CardArtifact_Id" });
            DropIndex("dbo.Cost", "UQX_Cost_Money_Dept_Potion");
            DropIndex("dbo.CardTypeToCards", new[] { "CardTypeId" });
            DropIndex("dbo.CardTypeToCards", new[] { "CardId" });
            DropIndex("dbo.CardSubTypeToCards", new[] { "CardSubTypeId" });
            DropIndex("dbo.CardSubTypeToCards", new[] { "CardId" });
            DropIndex("dbo.DominionExpansion", new[] { "Name" });
            DropIndex("dbo.CardArtifacts", new[] { "AdditionalCardId" });
            DropIndex("dbo.CardArtifacts", "UQX_CardAttribute_Name_DominionExpansionId");
            DropIndex("dbo.Card", new[] { "CostId" });
            DropIndex("dbo.Card", "UQX_Card_Name_DominionExpansionId");
            DropTable("dbo.CardArtifactCards");
            DropTable("dbo.Cost");
            DropTable("dbo.CardType");
            DropTable("dbo.CardTypeToCards");
            DropTable("dbo.CardSubTypes");
            DropTable("dbo.CardSubTypeToCards");
            DropTable("dbo.DominionExpansion");
            DropTable("dbo.CardArtifacts");
            DropTable("dbo.Card");
            DropTable("dbo.AdditionalCards");
        }
    }
}
