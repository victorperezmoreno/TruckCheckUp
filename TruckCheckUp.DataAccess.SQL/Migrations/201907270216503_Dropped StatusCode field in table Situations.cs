namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedStatusCodefieldintableSituations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Situations", "Description", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Situations", "StatusCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Situations", "StatusCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Situations", "Description", c => c.String(nullable: false));
        }
    }
}
