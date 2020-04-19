namespace DSG.BusinessEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDSGDatabase2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "Cost_Id", "dbo.Costs");
            DropForeignKey("dbo.Cards", "DominionExpansion_Id", "dbo.DominionExpansions");
            RenameTable(name: "dbo.Cards", newName: "Card");
            RenameTable(name: "dbo.Costs", newName: "Cost");
            RenameTable(name: "dbo.DominionExpansions", newName: "DominionExpansion");
            DropIndex("dbo.Card", new[] { "Cost_Id" });
            DropIndex("dbo.Card", new[] { "DominionExpansion_Id" });
            RenameColumn(table: "dbo.Card", name: "Cost_Id", newName: "CostId");
            RenameColumn(table: "dbo.Card", name: "DominionExpansion_Id", newName: "DominionExpansionId");
            AlterColumn("dbo.Card", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Card", "CostId", c => c.Int(nullable: false));
            AlterColumn("dbo.Card", "DominionExpansionId", c => c.Int(nullable: false));
            AlterColumn("dbo.DominionExpansion", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Card", new[] { "Name", "DominionExpansionId" }, unique: true, name: "UQX_Card_Name_DominionExpansionId");
            CreateIndex("dbo.Card", "CostId");
            CreateIndex("dbo.DominionExpansion", "Name", unique: true);
            AddForeignKey("dbo.Card", "CostId", "dbo.Cost", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Card", "DominionExpansionId", "dbo.DominionExpansion", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Card", "DominionExpansionId", "dbo.DominionExpansion");
            DropForeignKey("dbo.Card", "CostId", "dbo.Cost");
            DropIndex("dbo.DominionExpansion", new[] { "Name" });
            DropIndex("dbo.Card", new[] { "CostId" });
            DropIndex("dbo.Card", "UQX_Card_Name_DominionExpansionId");
            AlterColumn("dbo.DominionExpansion", "Name", c => c.String());
            AlterColumn("dbo.Card", "DominionExpansionId", c => c.Int());
            AlterColumn("dbo.Card", "CostId", c => c.Int());
            AlterColumn("dbo.Card", "Name", c => c.String());
            RenameColumn(table: "dbo.Card", name: "DominionExpansionId", newName: "DominionExpansion_Id");
            RenameColumn(table: "dbo.Card", name: "CostId", newName: "Cost_Id");
            CreateIndex("dbo.Card", "DominionExpansion_Id");
            CreateIndex("dbo.Card", "Cost_Id");
            AddForeignKey("dbo.Cards", "DominionExpansion_Id", "dbo.DominionExpansions", "Id");
            AddForeignKey("dbo.Cards", "Cost_Id", "dbo.Costs", "Id");
            RenameTable(name: "dbo.DominionExpansion", newName: "DominionExpansions");
            RenameTable(name: "dbo.Cost", newName: "Costs");
            RenameTable(name: "dbo.Card", newName: "Cards");
        }
    }
}
