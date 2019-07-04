namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedIsOKfieldinTruckInspectiontable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TruckInspections", "IsOK");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckInspections", "IsOK", c => c.Boolean(nullable: false));
        }
    }
}
