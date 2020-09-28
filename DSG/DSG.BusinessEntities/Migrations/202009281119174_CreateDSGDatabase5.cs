namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CardArtifact", "AmountOfAdditionalCards", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CardArtifact", "AmountOfAdditionalCards", c => c.Int(nullable: false));
        }
    }
}
