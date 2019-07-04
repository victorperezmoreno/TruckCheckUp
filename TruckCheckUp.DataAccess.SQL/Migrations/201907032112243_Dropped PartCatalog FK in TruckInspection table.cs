namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedPartCatalogFKinTruckInspectiontable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TruckInspections", name: "PartCatalogId", newName: "PartCatalog_Id");
            RenameIndex(table: "dbo.TruckInspections", name: "IX_PartCatalogId", newName: "IX_PartCatalog_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TruckInspections", name: "IX_PartCatalog_Id", newName: "IX_PartCatalogId");
            RenameColumn(table: "dbo.TruckInspections", name: "PartCatalog_Id", newName: "PartCatalogId");
        }
    }
}
