namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CommentDriver = c.String(maxLength: 100),
                        PartReportedId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartReporteds", t => t.PartReportedId)
                .Index(t => t.PartReportedId);
            
            CreateTable(
                "dbo.PartReporteds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ReportedDate = c.DateTime(nullable: false),
                        Mileage = c.Int(nullable: false),
                        DriverId = c.String(maxLength: 128),
                        TruckId = c.String(maxLength: 128),
                        PartCatalogId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.PartCatalogs", t => t.PartCatalogId)
                .ForeignKey("dbo.Trucks", t => t.TruckId)
                .Index(t => t.DriverId)
                .Index(t => t.TruckId)
                .Index(t => t.PartCatalogId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MechanicComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CommentDate = c.DateTime(nullable: false),
                        CommentMechanic = c.String(maxLength: 100),
                        PartReportedId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartReporteds", t => t.PartReportedId)
                .Index(t => t.PartReportedId);
            
            CreateTable(
                "dbo.PartCatalogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PartName = c.String(),
                        PartCategoryId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartCategories", t => t.PartCategoryId)
                .Index(t => t.PartCategoryId);
            
            CreateTable(
                "dbo.PartCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryPart = c.String(),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Situations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Status = c.Boolean(nullable: false),
                        StatusDate = c.DateTime(nullable: false),
                        PartReportedId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartReporteds", t => t.PartReportedId)
                .Index(t => t.PartReportedId);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VIN = c.String(nullable: false, maxLength: 30),
                        TruckNumber = c.Int(nullable: false),
                        Manufacturer = c.String(nullable: false, maxLength: 30),
                        Model = c.String(nullable: false, maxLength: 30),
                        Year = c.Int(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverComments", "PartReportedId", "dbo.PartReporteds");
            DropForeignKey("dbo.PartReporteds", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.Situations", "PartReportedId", "dbo.PartReporteds");
            DropForeignKey("dbo.PartReporteds", "PartCatalogId", "dbo.PartCatalogs");
            DropForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories");
            DropForeignKey("dbo.MechanicComments", "PartReportedId", "dbo.PartReporteds");
            DropForeignKey("dbo.PartReporteds", "DriverId", "dbo.Drivers");
            DropIndex("dbo.Situations", new[] { "PartReportedId" });
            DropIndex("dbo.PartCatalogs", new[] { "PartCategoryId" });
            DropIndex("dbo.MechanicComments", new[] { "PartReportedId" });
            DropIndex("dbo.PartReporteds", new[] { "PartCatalogId" });
            DropIndex("dbo.PartReporteds", new[] { "TruckId" });
            DropIndex("dbo.PartReporteds", new[] { "DriverId" });
            DropIndex("dbo.DriverComments", new[] { "PartReportedId" });
            DropTable("dbo.Trucks");
            DropTable("dbo.Situations");
            DropTable("dbo.PartCategories");
            DropTable("dbo.PartCatalogs");
            DropTable("dbo.MechanicComments");
            DropTable("dbo.Drivers");
            DropTable("dbo.PartReporteds");
            DropTable("dbo.DriverComments");
        }
    }
}
