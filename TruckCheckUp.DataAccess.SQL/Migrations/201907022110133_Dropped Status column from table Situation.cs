namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedStatuscolumnfromtableSituation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Situations", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Situations", "Status", c => c.Boolean(nullable: false));
        }
    }
}
