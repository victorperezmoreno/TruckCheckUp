namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedReportIdfieldinTruckInspectiontable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TruckInspections", "ReportId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckInspections", "ReportId", c => c.Int(nullable: false));
        }
    }
}
