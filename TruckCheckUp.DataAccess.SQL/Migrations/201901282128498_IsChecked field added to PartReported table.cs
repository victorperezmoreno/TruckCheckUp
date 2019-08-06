namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCheckedfieldaddedtoPartReportedtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartReporteds", "IsChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartReporteds", "IsChecked");
        }
    }
}
