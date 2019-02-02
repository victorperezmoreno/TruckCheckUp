namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveInactivestatusaddedtoDriverandTrucktables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Trucks", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PartCatalogs", "PartName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.PartCategories", "CategoryPart", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PartCategories", "CategoryPart", c => c.String());
            AlterColumn("dbo.PartCatalogs", "PartName", c => c.String());
            DropColumn("dbo.Trucks", "Status");
            DropColumn("dbo.Drivers", "Status");
        }
    }
}
