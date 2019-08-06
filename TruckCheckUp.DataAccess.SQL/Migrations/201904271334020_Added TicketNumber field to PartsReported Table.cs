namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTicketNumberfieldtoPartsReportedTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartReporteds", "TicketNumber", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.PartReporteds", "IsOK", c => c.Boolean(nullable: false));
            DropColumn("dbo.PartReporteds", "IsChecked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartReporteds", "IsChecked", c => c.Boolean(nullable: false));
            DropColumn("dbo.PartReporteds", "IsOK");
            DropColumn("dbo.PartReporteds", "TicketNumber");
        }
    }
}
