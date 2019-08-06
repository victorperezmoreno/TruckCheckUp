namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTruckYearTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TruckYears",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModelYear = c.Int(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trucks", "TruckYearId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Trucks", "TruckYearId");
            AddForeignKey("dbo.Trucks", "TruckYearId", "dbo.TruckYears", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trucks", "TruckYearId", "dbo.TruckYears");
            DropIndex("dbo.Trucks", new[] { "TruckYearId" });
            DropColumn("dbo.Trucks", "TruckYearId");
            DropTable("dbo.TruckYears");
        }
    }
}
