namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase10 : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.CardType", "IsSupplyType", c => c.Boolean(nullable: false));
            DropColumn("dbo.CardType", "IsKingdomCard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CardType", "IsKingdomCard", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.CardSubTypeToCards", "CardSubTypeId", "dbo.CardSubTypes");
            DropForeignKey("dbo.CardSubTypeToCards", "CardId", "dbo.Card");
            DropIndex("dbo.CardSubTypeToCards", new[] { "CardSubTypeId" });
            DropIndex("dbo.CardSubTypeToCards", new[] { "CardId" });
            DropColumn("dbo.CardType", "IsSupplyType");
            DropTable("dbo.CardSubTypes");
            DropTable("dbo.CardSubTypeToCards");
        }
    }
}
