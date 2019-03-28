namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedManufacturerFKtoModelTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckModels", "TruckManufacturerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TruckModels", "TruckManufacturerId");
            AddForeignKey("dbo.TruckModels", "TruckManufacturerId", "dbo.TruckManufacturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TruckModels", "TruckManufacturerId", "dbo.TruckManufacturers");
            DropIndex("dbo.TruckModels", new[] { "TruckManufacturerId" });
            DropColumn("dbo.TruckModels", "TruckManufacturerId");
        }
    }
}
