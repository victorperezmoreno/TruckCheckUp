namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredNotationforDriverClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Drivers", "FirstName", c => c.String(maxLength: 30));
            AlterColumn("dbo.Drivers", "LastName", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drivers", "LastName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Drivers", "FirstName", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
