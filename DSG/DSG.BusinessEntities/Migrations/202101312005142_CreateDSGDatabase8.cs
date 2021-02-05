namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase8 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GenerationProfiles", "UQX_GenerationProfile_Name");
            AddColumn("dbo.GenerationProfiles", "ValidationStep", c => c.Int(nullable: false));
            AddColumn("dbo.GenerationProfiles", "ValidatesOnTargetUpdated", c => c.Boolean(nullable: false));
            AlterColumn("dbo.GenerationProfiles", "Name", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.GenerationProfiles", "Name", unique: true, name: "UQX_GenerationProfile_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GenerationProfiles", "UQX_GenerationProfile_Name");
            AlterColumn("dbo.GenerationProfiles", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.GenerationProfiles", "ValidatesOnTargetUpdated");
            DropColumn("dbo.GenerationProfiles", "ValidationStep");
            CreateIndex("dbo.GenerationProfiles", "Name", unique: true, name: "UQX_GenerationProfile_Name");
        }
    }
}
