namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase13 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "[dbo].[CardAttribute]", newName: "CardArtifact");
            RenameTable(name: "[dbo].[CardAttributeCards]", newName: "CardArtifactCards");
            RenameColumn(table: "[dbo].[CardArtifactCards]", name: "[CardAttributId]", newName: "CardArtifactId");
            RenameIndex(table: "[dbo].[CardArtifactCards]", name: "[IX_CardAttributId]", newName: "IX_CardArtifact_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CardArtifactCards", name: "IX_CardArtifact_Id", newName: "[IX_CardAttributId]");
            RenameColumn(table: "dbo.CardArtifactCards", name: "CardArtifactId", newName: "CardAttributId");
            RenameTable(name: "dbo.CardArtifactCards", newName: "CardAttributeCards");
            RenameTable(name: "dbo.CardArtifact", newName: "CardAttribute");
        }
    }
}
