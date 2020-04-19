namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cost", "Money", c => c.Int(nullable: false));
            AlterColumn("dbo.Cost", "Potion", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Cost", "Dept", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cost", "Dept", c => c.Int());
            AlterColumn("dbo.Cost", "Potion", c => c.Int());
            AlterColumn("dbo.Cost", "Money", c => c.Int());
        }
    }
}
