namespace TruckCheckUp.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedCommentDateinTableMechanicComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Situations", "PartReportedId", "dbo.TruckInspections");
            DropIndex("dbo.Situations", new[] { "PartReportedId" });
            DropColumn("dbo.Situations", "PartReportedId");
            DropColumn("dbo.MechanicComments", "CommentDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MechanicComments", "CommentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Situations", "PartReportedId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Situations", "PartReportedId");
            AddForeignKey("dbo.Situations", "PartReportedId", "dbo.TruckInspections", "Id");
        }
    }
}
