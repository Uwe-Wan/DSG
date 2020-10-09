namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cost", "UQX_Cost_Money_Dept_Potion");
            AlterColumn("dbo.Cost", "Money", c => c.Int());
            AlterColumn("dbo.Cost", "Dept", c => c.Int());
            CreateIndex("dbo.Cost", new[] { "Money", "Potion", "Dept" }, unique: true, name: "UQX_Cost_Money_Dept_Potion");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Cost", "UQX_Cost_Money_Dept_Potion");
            AlterColumn("dbo.Cost", "Dept", c => c.Int(nullable: false));
            AlterColumn("dbo.Cost", "Money", c => c.Int(nullable: false));
            CreateIndex("dbo.Cost", new[] { "Money", "Potion", "Dept" }, unique: true, name: "UQX_Cost_Money_Dept_Potion");
        }
    }
}
