namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cost", "Money", c => c.Int(defaultValue: 0));
            AlterColumn("dbo.Cost", "Potion", c => c.Boolean(defaultValue: false));
            AlterColumn("dbo.Cost", "Dept", c => c.Int(defaultValue: 0));
        }
        
        public override void Down()
        {
        }
    }
}
