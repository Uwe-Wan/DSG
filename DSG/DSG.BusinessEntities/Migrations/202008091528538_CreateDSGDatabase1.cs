namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CardArtifacts", "AmountOfAdditionalCards", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CardArtifacts", "AmountOfAdditionalCards");
        }
    }
}
