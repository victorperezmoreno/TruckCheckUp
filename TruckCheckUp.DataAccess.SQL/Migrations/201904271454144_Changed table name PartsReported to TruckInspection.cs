namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedtablenamePartsReportedtoTruckInspection : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PartReporteds", newName: "TruckInspections");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TruckInspections", newName: "PartReporteds");
        }
    }
}
