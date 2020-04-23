namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Cost", new[] { "Money", "Potion", "Dept" }, unique: true, name: "UQX_Cost_Money_Dept_Potion");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Cost", "UQX_Cost_Money_Dept_Potion");
        }
    }
}
