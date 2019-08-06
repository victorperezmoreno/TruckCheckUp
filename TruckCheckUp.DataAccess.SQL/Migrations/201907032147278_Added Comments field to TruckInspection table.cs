namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentsfieldtoTruckInspectiontable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckInspections", "Comments", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TruckInspections", "Comments");
        }
    }
}
