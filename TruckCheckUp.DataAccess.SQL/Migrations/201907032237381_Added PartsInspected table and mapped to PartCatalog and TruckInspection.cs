namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPartsInspectedtableandmappedtoPartCatalogandTruckInspection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartsInspecteds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsOK = c.Boolean(nullable: false),
                        PartCatalogId = c.String(maxLength: 128),
                        TruckInspectionId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartCatalogs", t => t.PartCatalogId)
                .ForeignKey("dbo.TruckInspections", t => t.TruckInspectionId)
                .Index(t => t.PartCatalogId)
                .Index(t => t.TruckInspectionId);
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckInspections", "PartCatalog_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PartsInspecteds", "TruckInspectionId", "dbo.TruckInspections");
            DropForeignKey("dbo.PartsInspecteds", "PartCatalogId", "dbo.PartCatalogs");
            DropIndex("dbo.PartsInspecteds", new[] { "TruckInspectionId" });
            DropIndex("dbo.PartsInspecteds", new[] { "PartCatalogId" });
            DropTable("dbo.PartsInspecteds");
        }
    }
}
