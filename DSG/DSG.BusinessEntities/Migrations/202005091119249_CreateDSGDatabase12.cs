namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase12 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CardAttribute", "UQX_CardAttribute_Name_DominionExpansionId");
            DropIndex("dbo.CardAttribute", "UQX_Card_Name_DominionExpansionId");
            AlterColumn("dbo.CardAttribute", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.CardAttribute", new[] { "Name", "DominionExpansionId" }, unique: true, name: "UQX_CardAttribute_Name_DominionExpansionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CardAttribute", "UQX_CardAttribute_Name_DominionExpansionId");
            AlterColumn("dbo.CardAttribute", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.CardAttribute", "DominionExpansionId", unique: true, name: "UQX_Card_Name_DominionExpansionId");
            CreateIndex("dbo.CardAttribute", "Name", unique: true, name: "UQX_CardAttribute_Name_DominionExpansionId");
        }
    }
}
