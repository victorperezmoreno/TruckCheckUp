namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSituationFKtoMechanicCommentstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicComments", "SituationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MechanicComments", "SituationId");
            AddForeignKey("dbo.MechanicComments", "SituationId", "dbo.Situations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MechanicComments", "SituationId", "dbo.Situations");
            DropIndex("dbo.MechanicComments", new[] { "SituationId" });
            DropColumn("dbo.MechanicComments", "SituationId");
        }
    }
}
