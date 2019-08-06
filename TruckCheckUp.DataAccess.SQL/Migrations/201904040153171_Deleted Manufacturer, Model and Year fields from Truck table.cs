namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedManufacturerModelandYearfieldsfromTrucktable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trucks", "Manufacturer");
            DropColumn("dbo.Trucks", "Model");
            DropColumn("dbo.Trucks", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trucks", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Trucks", "Model", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Trucks", "Manufacturer", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
