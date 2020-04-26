namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CardTypeCards",
                c => new
                    {
                        CardTypeId = c.Int(nullable: false),
                        CardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardTypeId, t.CardId })
                .ForeignKey("dbo.CardType", t => t.CardTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardTypeId)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardTypeCards", "CardId", "dbo.Card");
            DropForeignKey("dbo.CardTypeCards", "CardTypeId", "dbo.CardType");
            DropIndex("dbo.CardTypeCards", new[] { "CardId" });
            DropIndex("dbo.CardTypeCards", new[] { "CardTypeId" });
            DropTable("dbo.CardTypeCards");
            DropTable("dbo.CardType");
        }
    }
}
