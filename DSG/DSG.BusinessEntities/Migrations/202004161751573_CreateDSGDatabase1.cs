namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cards", "ExpansionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "ExpansionId", c => c.Int(nullable: false));
        }
    }
}
