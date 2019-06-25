namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFKforalltablesfromPartReportedtoTruckInspection : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MechanicComments", name: "PartReportedId", newName: "TruckInspectionId");
            RenameIndex(table: "dbo.MechanicComments", name: "IX_PartReportedId", newName: "IX_TruckInspectionId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MechanicComments", name: "IX_TruckInspectionId", newName: "IX_PartReportedId");
            RenameColumn(table: "dbo.MechanicComments", name: "TruckInspectionId", newName: "PartReportedId");
        }
    }
}
