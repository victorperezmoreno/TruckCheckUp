namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedPartReportedIdFKintableDriverComment : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DriverComments", name: "PartReportedId", newName: "TruckInspection_Id");
            RenameIndex(table: "dbo.DriverComments", name: "IX_PartReportedId", newName: "IX_TruckInspection_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DriverComments", name: "IX_TruckInspection_Id", newName: "IX_PartReportedId");
            RenameColumn(table: "dbo.DriverComments", name: "TruckInspection_Id", newName: "PartReportedId");
        }
    }
}
