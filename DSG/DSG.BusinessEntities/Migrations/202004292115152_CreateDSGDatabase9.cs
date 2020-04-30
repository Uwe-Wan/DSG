namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardType", "IsKingdomCard", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardType", "IsKingdomCard");
        }
    }
}
