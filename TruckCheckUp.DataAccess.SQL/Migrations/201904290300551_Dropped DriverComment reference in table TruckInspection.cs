namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedDriverCommentreferenceintableTruckInspection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DriverComments", "TruckInspection_Id", "dbo.TruckInspections");
            DropIndex("dbo.DriverComments", new[] { "TruckInspection_Id" });
            DropColumn("dbo.DriverComments", "TruckInspection_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DriverComments", "TruckInspection_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DriverComments", "TruckInspection_Id");
            AddForeignKey("dbo.DriverComments", "TruckInspection_Id", "dbo.TruckInspections", "Id");
        }
    }
}
