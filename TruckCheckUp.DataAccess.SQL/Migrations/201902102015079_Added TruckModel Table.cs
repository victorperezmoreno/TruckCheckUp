namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTruckModelTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TruckModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModelDescription = c.String(nullable: false, maxLength: 30),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trucks", "TruckModelId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Trucks", "TruckModelId");
            AddForeignKey("dbo.Trucks", "TruckModelId", "dbo.TruckModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trucks", "TruckModelId", "dbo.TruckModels");
            DropIndex("dbo.Trucks", new[] { "TruckModelId" });
            DropColumn("dbo.Trucks", "TruckModelId");
            DropTable("dbo.TruckModels");
        }
    }
}
