namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GenerationProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PropabilityForShelters = c.Int(nullable: false),
                        PropabilityForPlatinumAndColony = c.Int(nullable: false),
                        PropabilityForNonSupplyCardsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PropabilityForNonSupplyCards", t => t.PropabilityForNonSupplyCardsId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "UQX_GenerationProfile_Name")
                .Index(t => t.PropabilityForNonSupplyCardsId);
            
            CreateTable(
                "dbo.PropabilityForNonSupplyCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropabilityForOne = c.Int(nullable: false),
                        PropabilityForTwo = c.Int(nullable: false),
                        PropabilityForThree = c.Int(nullable: false),
                        PropabilityForFour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SelectedExpansionToGenerationProfiles",
                c => new
                    {
                        DominionExpansionId = c.Int(nullable: false),
                        GenerationProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DominionExpansionId, t.GenerationProfileId })
                .ForeignKey("dbo.DominionExpansion", t => t.DominionExpansionId, cascadeDelete: true)
                .ForeignKey("dbo.GenerationProfiles", t => t.GenerationProfileId, cascadeDelete: true)
                .Index(t => t.DominionExpansionId)
                .Index(t => t.GenerationProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SelectedExpansionToGenerationProfiles", "GenerationProfileId", "dbo.GenerationProfiles");
            DropForeignKey("dbo.SelectedExpansionToGenerationProfiles", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.GenerationProfiles", "PropabilityForNonSupplyCardsId", "dbo.PropabilityForNonSupplyCards");
            DropIndex("dbo.SelectedExpansionToGenerationProfiles", new[] { "GenerationProfileId" });
            DropIndex("dbo.SelectedExpansionToGenerationProfiles", new[] { "DominionExpansionId" });
            DropIndex("dbo.GenerationProfiles", new[] { "PropabilityForNonSupplyCardsId" });
            DropIndex("dbo.GenerationProfiles", "UQX_GenerationProfile_Name");
            DropTable("dbo.SelectedExpansionToGenerationProfiles");
            DropTable("dbo.PropabilityForNonSupplyCards");
            DropTable("dbo.GenerationProfiles");
        }
    }
}
