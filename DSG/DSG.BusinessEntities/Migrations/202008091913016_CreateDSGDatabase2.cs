namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CardArtifacts", "DominionExpansionId", "dbo.DominionExpansion");
            DropIndex("dbo.CardArtifacts", "UQX_CardAttribute_Name_DominionExpansionId");
            CreateTable(
                "dbo.CardArtifactToExpansions",
                c => new
                    {
                        DominionExpansionId = c.Int(nullable: false),
                        CardArtifactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DominionExpansionId, t.CardArtifactId })
                .ForeignKey("dbo.CardArtifacts", t => t.CardArtifactId, cascadeDelete: true)
                .ForeignKey("dbo.DominionExpansion", t => t.DominionExpansionId, cascadeDelete: true)
                .Index(t => t.DominionExpansionId)
                .Index(t => t.CardArtifactId);
            
            CreateIndex("dbo.CardArtifacts", "Name", unique: true, name: "UQX_CardArtifact_Name");
            DropColumn("dbo.CardArtifacts", "DominionExpansionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CardArtifacts", "DominionExpansionId", c => c.Int());
            DropForeignKey("dbo.CardArtifactToExpansions", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.CardArtifactToExpansions", "CardArtifactId", "dbo.CardArtifacts");
            DropIndex("dbo.CardArtifactToExpansions", new[] { "CardArtifactId" });
            DropIndex("dbo.CardArtifactToExpansions", new[] { "DominionExpansionId" });
            DropIndex("dbo.CardArtifacts", "UQX_CardArtifact_Name");
            DropTable("dbo.CardArtifactToExpansions");
            CreateIndex("dbo.CardArtifacts", new[] { "Name", "DominionExpansionId" }, unique: true, name: "UQX_CardAttribute_Name_DominionExpansionId");
            AddForeignKey("dbo.CardArtifacts", "DominionExpansionId", "dbo.DominionExpansion", "Id");
        }
    }
}
