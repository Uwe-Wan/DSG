namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CardArtifacts", newName: "CardArtifact");
            DropForeignKey("dbo.CardArtifactCards", "CardArtifact_Id", "dbo.CardArtifacts");
            DropForeignKey("dbo.CardArtifactCards", "Card_Id", "dbo.Card");
            DropForeignKey("dbo.CardArtifactToExpansions", "CardArtifactId", "dbo.CardArtifacts");
            DropForeignKey("dbo.CardArtifactToExpansions", "DominionExpansionId", "dbo.DominionExpansion");
            DropIndex("dbo.CardArtifact", "UQX_CardArtifact_Name");
            DropIndex("dbo.CardArtifactToExpansions", new[] { "DominionExpansionId" });
            DropIndex("dbo.CardArtifactToExpansions", new[] { "CardArtifactId" });
            DropIndex("dbo.CardArtifactCards", new[] { "CardArtifact_Id" });
            DropIndex("dbo.CardArtifactCards", new[] { "Card_Id" });
            CreateTable(
                "dbo.CardArtifactToCards",
                c => new
                    {
                        CardId = c.Int(nullable: false),
                        CardArtifactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardId, t.CardArtifactId })
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: false)
                .ForeignKey("dbo.CardArtifact", t => t.CardArtifactId, cascadeDelete: false)
                .Index(t => t.CardId)
                .Index(t => t.CardArtifactId);
            
            AddColumn("dbo.CardArtifact", "DominionExpansionId", c => c.Int(nullable: false));
            CreateIndex("dbo.CardArtifact", new[] { "Name", "DominionExpansionId" }, unique: true, name: "UQX_CardArtifact_Name_DominionExpansion");
            AddForeignKey("dbo.CardArtifact", "DominionExpansionId", "dbo.DominionExpansion", "Id", cascadeDelete: true);
            DropTable("dbo.CardArtifactToExpansions");
            DropTable("dbo.CardArtifactCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CardArtifactCards",
                c => new
                    {
                        CardArtifact_Id = c.Int(nullable: false),
                        Card_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardArtifact_Id, t.Card_Id });
            
            CreateTable(
                "dbo.CardArtifactToExpansions",
                c => new
                    {
                        DominionExpansionId = c.Int(nullable: false),
                        CardArtifactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DominionExpansionId, t.CardArtifactId });
            
            DropForeignKey("dbo.CardArtifactToCards", "CardArtifactId", "dbo.CardArtifact");
            DropForeignKey("dbo.CardArtifact", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.CardArtifactToCards", "CardId", "dbo.Card");
            DropIndex("dbo.CardArtifact", "UQX_CardArtifact_Name_DominionExpansion");
            DropIndex("dbo.CardArtifactToCards", new[] { "CardArtifactId" });
            DropIndex("dbo.CardArtifactToCards", new[] { "CardId" });
            DropColumn("dbo.CardArtifact", "DominionExpansionId");
            DropTable("dbo.CardArtifactToCards");
            CreateIndex("dbo.CardArtifactCards", "Card_Id");
            CreateIndex("dbo.CardArtifactCards", "CardArtifact_Id");
            CreateIndex("dbo.CardArtifactToExpansions", "CardArtifactId");
            CreateIndex("dbo.CardArtifactToExpansions", "DominionExpansionId");
            CreateIndex("dbo.CardArtifact", "Name", unique: true, name: "UQX_CardArtifact_Name");
            AddForeignKey("dbo.CardArtifactToExpansions", "DominionExpansionId", "dbo.DominionExpansion", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CardArtifactToExpansions", "CardArtifactId", "dbo.CardArtifacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CardArtifactCards", "Card_Id", "dbo.Card", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CardArtifactCards", "CardArtifact_Id", "dbo.CardArtifacts", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.CardArtifact", newName: "CardArtifacts");
        }
    }
}
