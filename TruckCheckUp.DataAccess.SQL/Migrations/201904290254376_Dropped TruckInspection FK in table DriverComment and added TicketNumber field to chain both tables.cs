namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedTruckInspectionFKintableDriverCommentandaddedTicketNumberfieldtochainbothtables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DriverComments", "TicketNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DriverComments", "TicketNumber");
        }
    }
}
