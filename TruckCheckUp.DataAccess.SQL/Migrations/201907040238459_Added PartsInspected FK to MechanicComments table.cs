namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPartsInspectedFKtoMechanicCommentstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicComments", "PartsInspectedId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MechanicComments", "PartsInspectedId");
            AddForeignKey("dbo.MechanicComments", "PartsInspectedId", "dbo.PartsInspecteds", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MechanicComments", "PartsInspectedId", "dbo.PartsInspecteds");
            DropIndex("dbo.MechanicComments", new[] { "PartsInspectedId" });
            DropColumn("dbo.MechanicComments", "PartsInspectedId");
        }
    }
}
