namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTicketNumbercolumnwithnoidentityinTableTruckInspection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckInspections", "TicketNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TruckInspections", "TicketNumber");
        }
    }
}
