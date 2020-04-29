namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CardType", "CardId", "dbo.Card");
            DropIndex("dbo.CardType", new[] { "CardId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.CardType", "CardId");
            AddForeignKey("dbo.CardType", "CardId", "dbo.Card", "Id");
        }
    }
}
