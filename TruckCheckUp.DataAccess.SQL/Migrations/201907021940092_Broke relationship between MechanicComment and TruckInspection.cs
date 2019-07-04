namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrokerelationshipbetweenMechanicCommentandTruckInspection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MechanicComments", "TruckInspectionId", "dbo.TruckInspections");
            DropIndex("dbo.MechanicComments", new[] { "TruckInspectionId" });
            AddColumn("dbo.DriverComments", "ReportId", c => c.Int(nullable: false));
            DropColumn("dbo.DriverComments", "TicketNumber");
            DropColumn("dbo.MechanicComments", "TruckInspectionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MechanicComments", "TruckInspectionId", c => c.String(maxLength: 128));
            AddColumn("dbo.DriverComments", "TicketNumber", c => c.Int(nullable: false));
            DropColumn("dbo.DriverComments", "ReportId");
            CreateIndex("dbo.MechanicComments", "TruckInspectionId");
            AddForeignKey("dbo.MechanicComments", "TruckInspectionId", "dbo.TruckInspections", "Id");
        }
    }
}
