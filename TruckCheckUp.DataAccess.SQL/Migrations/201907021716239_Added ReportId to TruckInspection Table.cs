namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReportIdtoTruckInspectionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckInspections", "ReportId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TruckInspections", "ReportId");
        }
    }
}
