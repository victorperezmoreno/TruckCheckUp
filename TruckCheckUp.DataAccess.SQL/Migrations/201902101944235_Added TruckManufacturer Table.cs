namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTruckManufacturerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TruckManufacturers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ManufacturerDescription = c.String(nullable: false, maxLength: 30),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trucks", "TruckManufacturerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Trucks", "TruckManufacturerId");
            AddForeignKey("dbo.Trucks", "TruckManufacturerId", "dbo.TruckManufacturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trucks", "TruckManufacturerId", "dbo.TruckManufacturers");
            DropIndex("dbo.Trucks", new[] { "TruckManufacturerId" });
            DropColumn("dbo.Trucks", "TruckManufacturerId");
            DropTable("dbo.TruckManufacturers");
        }
    }
}
