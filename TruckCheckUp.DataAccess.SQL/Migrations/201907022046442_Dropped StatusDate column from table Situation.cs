namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedStatusDatecolumnfromtableSituation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Situations", "StatusDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Situations", "StatusDate", c => c.DateTime(nullable: false));
        }
    }
}
