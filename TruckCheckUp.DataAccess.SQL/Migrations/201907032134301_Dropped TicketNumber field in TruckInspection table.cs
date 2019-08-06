namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedTicketNumberfieldinTruckInspectiontable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TruckInspections", "TicketNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckInspections", "TicketNumber", c => c.Int(nullable: false));
        }
    }
}
