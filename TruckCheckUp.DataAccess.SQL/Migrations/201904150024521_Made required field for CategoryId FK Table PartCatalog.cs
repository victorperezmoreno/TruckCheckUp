namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaderequiredfieldforCategoryIdFKTablePartCatalog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories");
            DropIndex("dbo.PartCatalogs", new[] { "PartCategoryId" });
            AlterColumn("dbo.PartCatalogs", "PartCategoryId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.PartCatalogs", "PartCategoryId");
            AddForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories");
            DropIndex("dbo.PartCatalogs", new[] { "PartCategoryId" });
            AlterColumn("dbo.PartCatalogs", "PartCategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PartCatalogs", "PartCategoryId");
            AddForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories", "Id");
        }
    }
}
