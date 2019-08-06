namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedSituationFKfromMechanicCommentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Status = c.Boolean(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TruckInspections",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Mileage = c.Int(nullable: false),
                        DriverId = c.String(maxLength: 128),
                        TruckId = c.String(maxLength: 128),
                        Comments = c.String(maxLength: 100),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .ForeignKey("dbo.Trucks", t => t.TruckId)
                .Index(t => t.DriverId)
                .Index(t => t.TruckId);
            
            CreateTable(
                "dbo.PartsInspecteds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsOK = c.Boolean(nullable: false),
                        PartCatalogId = c.String(maxLength: 128),
                        TruckInspectionId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartCatalogs", t => t.PartCatalogId)
                .ForeignKey("dbo.TruckInspections", t => t.TruckInspectionId)
                .Index(t => t.PartCatalogId)
                .Index(t => t.TruckInspectionId);
            
            CreateTable(
                "dbo.MechanicComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CommentMechanic = c.String(maxLength: 100),
                        PartsInspectedId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartsInspecteds", t => t.PartsInspectedId)
                .Index(t => t.PartsInspectedId);
            
            CreateTable(
                "dbo.PartCatalogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PartName = c.String(nullable: false, maxLength: 30),
                        PartCategoryId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartCategories", t => t.PartCategoryId, cascadeDelete: true)
                .Index(t => t.PartCategoryId);
            
            CreateTable(
                "dbo.PartCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryPart = c.String(nullable: false, maxLength: 30),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VIN = c.String(nullable: false, maxLength: 30),
                        TruckNumber = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        TruckManufacturerId = c.String(maxLength: 128),
                        TruckModelId = c.String(maxLength: 128),
                        TruckYearId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TruckManufacturers", t => t.TruckManufacturerId)
                .ForeignKey("dbo.TruckModels", t => t.TruckModelId)
                .ForeignKey("dbo.TruckYears", t => t.TruckYearId)
                .Index(t => t.TruckManufacturerId)
                .Index(t => t.TruckModelId)
                .Index(t => t.TruckYearId);
            
            CreateTable(
                "dbo.TruckManufacturers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ManufacturerDescription = c.String(nullable: false, maxLength: 30),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TruckModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModelDescription = c.String(nullable: false, maxLength: 30),
                        TruckManufacturerId = c.String(maxLength: 128),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TruckManufacturers", t => t.TruckManufacturerId)
                .Index(t => t.TruckManufacturerId);
            
            CreateTable(
                "dbo.TruckYears",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModelYear = c.Int(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Situations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StatusCode = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TruckInspections", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.Trucks", "TruckYearId", "dbo.TruckYears");
            DropForeignKey("dbo.Trucks", "TruckModelId", "dbo.TruckModels");
            DropForeignKey("dbo.Trucks", "TruckManufacturerId", "dbo.TruckManufacturers");
            DropForeignKey("dbo.TruckModels", "TruckManufacturerId", "dbo.TruckManufacturers");
            DropForeignKey("dbo.PartsInspecteds", "TruckInspectionId", "dbo.TruckInspections");
            DropForeignKey("dbo.PartsInspecteds", "PartCatalogId", "dbo.PartCatalogs");
            DropForeignKey("dbo.PartCatalogs", "PartCategoryId", "dbo.PartCategories");
            DropForeignKey("dbo.MechanicComments", "PartsInspectedId", "dbo.PartsInspecteds");
            DropForeignKey("dbo.TruckInspections", "DriverId", "dbo.Drivers");
            DropIndex("dbo.TruckModels", new[] { "TruckManufacturerId" });
            DropIndex("dbo.Trucks", new[] { "TruckYearId" });
            DropIndex("dbo.Trucks", new[] { "TruckModelId" });
            DropIndex("dbo.Trucks", new[] { "TruckManufacturerId" });
            DropIndex("dbo.PartCatalogs", new[] { "PartCategoryId" });
            DropIndex("dbo.MechanicComments", new[] { "PartsInspectedId" });
            DropIndex("dbo.PartsInspecteds", new[] { "TruckInspectionId" });
            DropIndex("dbo.PartsInspecteds", new[] { "PartCatalogId" });
            DropIndex("dbo.TruckInspections", new[] { "TruckId" });
            DropIndex("dbo.TruckInspections", new[] { "DriverId" });
            DropTable("dbo.Situations");
            DropTable("dbo.TruckYears");
            DropTable("dbo.TruckModels");
            DropTable("dbo.TruckManufacturers");
            DropTable("dbo.Trucks");
            DropTable("dbo.PartCategories");
            DropTable("dbo.PartCatalogs");
            DropTable("dbo.MechanicComments");
            DropTable("dbo.PartsInspecteds");
            DropTable("dbo.TruckInspections");
            DropTable("dbo.Drivers");
        }
    }
}
