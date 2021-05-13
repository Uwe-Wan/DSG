namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SelectedExpansionToGenerationProfiles", "Weight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SelectedExpansionToGenerationProfiles", "Weight");
        }
    }
}
