namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusCodeandDescriptionfieldstoSituationtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Situations", "StatusCode", c => c.Int(nullable: false));
            AddColumn("dbo.Situations", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Situations", "Description");
            DropColumn("dbo.Situations", "StatusCode");
        }
    }
}
