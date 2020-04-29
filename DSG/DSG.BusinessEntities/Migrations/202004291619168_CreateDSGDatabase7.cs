namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase7 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CardTypeToCards", "CardId");
            AddForeignKey("dbo.CardTypeToCards", "CardId", "dbo.Card", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardTypeToCards", "CardId", "dbo.Card");
            DropIndex("dbo.CardTypeToCards", new[] { "CardId" });
        }
    }
}
