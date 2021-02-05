namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GenerationProfiles", "ValidationStep");
            DropColumn("dbo.GenerationProfiles", "ValidatesOnTargetUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GenerationProfiles", "ValidatesOnTargetUpdated", c => c.Boolean(nullable: false));
            AddColumn("dbo.GenerationProfiles", "ValidationStep", c => c.Int(nullable: false));
        }
    }
}
