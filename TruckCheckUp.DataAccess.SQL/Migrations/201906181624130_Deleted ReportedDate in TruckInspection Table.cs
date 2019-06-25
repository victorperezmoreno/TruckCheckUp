namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedReportedDateinTruckInspectionTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TruckInspections", "ReportedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckInspections", "ReportedDate", c => c.DateTime(nullable: false));
        }
    }
}
