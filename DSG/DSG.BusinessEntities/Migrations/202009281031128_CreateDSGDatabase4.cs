namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdditionalCards", "MaxCost", c => c.Int());
            AddColumn("dbo.AdditionalCards", "MinCost", c => c.Int());
            CreateIndex("dbo.AdditionalCards", new[] { "AlreadyIncludedCard", "MaxCost", "MinCost" }, unique: true, name: "UQX_AdditionalCards_AlreadyIncludedCard_MaxCost_MinCost");
            DropColumn("dbo.AdditionalCards", "MaxCosts");
            DropColumn("dbo.AdditionalCards", "MinCosts");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdditionalCards", "MinCosts", c => c.Int());
            AddColumn("dbo.AdditionalCards", "MaxCosts", c => c.Int());
            DropIndex("dbo.AdditionalCards", "UQX_AdditionalCards_AlreadyIncludedCard_MaxCost_MinCost");
            DropColumn("dbo.AdditionalCards", "MinCost");
            DropColumn("dbo.AdditionalCards", "MaxCost");
        }
    }
}
