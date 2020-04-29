namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CardTypeCards", "CardTypeId", "dbo.CardType");
            DropForeignKey("dbo.CardTypeCards", "CardId", "dbo.Card");
            DropIndex("dbo.CardTypeCards", new[] { "CardTypeId" });
            DropIndex("dbo.CardTypeCards", new[] { "CardId" });
            CreateTable(
                "dbo.CardTypeToCards",
                c => new
                    {
                        CardId = c.Int(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardId, t.CardTypeId })
                .ForeignKey("dbo.CardType", t => t.CardTypeId, cascadeDelete: true)
                .Index(t => t.CardTypeId);            
            DropTable("dbo.CardTypeCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CardTypeCards",
                c => new
                    {
                        CardTypeId = c.Int(nullable: false),
                        CardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardTypeId, t.CardId });
            
            DropForeignKey("dbo.CardTypeToCards", "CardTypeId", "dbo.CardType");
            DropForeignKey("dbo.CardType", "CardId", "dbo.Card");
            DropIndex("dbo.CardTypeToCards", new[] { "CardTypeId" });
            DropIndex("dbo.CardType", new[] { "CardId" });
            DropColumn("dbo.CardType", "CardId");
            DropTable("dbo.CardTypeToCards");
            CreateIndex("dbo.CardTypeCards", "CardId");
            CreateIndex("dbo.CardTypeCards", "CardTypeId");
            AddForeignKey("dbo.CardTypeCards", "CardId", "dbo.Card", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CardTypeCards", "CardTypeId", "dbo.CardType", "Id", cascadeDelete: true);
        }
    }
}
