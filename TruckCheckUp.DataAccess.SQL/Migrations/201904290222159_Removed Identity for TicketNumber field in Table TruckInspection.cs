namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIdentityforTicketNumberfieldinTableTruckInspection : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TruckInspections", "TicketNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TruckInspections", "TicketNumber", c => c.Int(nullable: false, identity: true));
        }
    }
}
