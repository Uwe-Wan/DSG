namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalCard",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AlreadyIncludedCard = c.Boolean(nullable: false),
                    MaxCosts = c.Int(),
                    MinCosts = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CardAttribute",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DominionExpansionId = c.Int(),
                        AdditionalCardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalCard", t => t.AdditionalCardId)
                .ForeignKey("dbo.DominionExpansion", t => t.DominionExpansionId)
                .Index(t => t.Name, unique: true, name: "UQX_CardAttribute_Name_DominionExpansionId")
                .Index(t => t.DominionExpansionId, unique: true, name: "UQX_Card_Name_DominionExpansionId")
                .Index(t => t.AdditionalCardId);
            
            CreateTable(
                "dbo.CardAttributeCards",
                c => new
                    {
                        CardAttributId = c.Int(nullable: false),
                        CardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardAttributId, t.CardId })
                .ForeignKey("dbo.CardAttribute", t => t.CardAttributId, cascadeDelete: true)
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardAttributId)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardAttribute", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.CardAttributeCards", "CardId", "dbo.Card");
            DropForeignKey("dbo.CardAttributeCards", "CardAttributId", "dbo.CardAttribute");
            DropForeignKey("dbo.CardAttribute", "AdditionalCardId", "dbo.AdditionalCard");
            DropIndex("dbo.CardAttributeCards", new[] { "CardId" });
            DropIndex("dbo.CardAttributeCards", new[] { "CardAttributId" });
            DropIndex("dbo.CardAttribute", new[] { "AdditionalCardId" });
            DropIndex("dbo.CardAttribute", "UQX_Card_Name_DominionExpansionId");
            DropIndex("dbo.CardAttribute", "UQX_CardAttribute_Name_DominionExpansionId");
            DropTable("dbo.CardAttributeCards");
            DropTable("dbo.AdditionalCard");
            DropTable("dbo.CardAttribute");
        }
    }
}
