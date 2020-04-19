namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExpansionId = c.Int(nullable: false),
                        Cost_Id = c.Int(),
                        DominionExpansion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Costs", t => t.Cost_Id)
                .ForeignKey("dbo.DominionExpansions", t => t.DominionExpansion_Id)
                .Index(t => t.Cost_Id)
                .Index(t => t.DominionExpansion_Id);
            
            CreateTable(
                "dbo.Costs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Money = c.Int(),
                        Potion = c.Int(),
                        Dept = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DominionExpansions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "DominionExpansion_Id", "dbo.DominionExpansions");
            DropForeignKey("dbo.Cards", "Cost_Id", "dbo.Costs");
            DropIndex("dbo.Cards", new[] { "DominionExpansion_Id" });
            DropIndex("dbo.Cards", new[] { "Cost_Id" });
            DropTable("dbo.DominionExpansions");
            DropTable("dbo.Costs");
            DropTable("dbo.Cards");
        }
    }
}
