namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusfieldtosituationtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Situations", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Situations", "Status");
        }
    }
}
