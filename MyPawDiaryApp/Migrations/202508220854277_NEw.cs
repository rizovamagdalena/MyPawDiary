namespace MyPawDiaryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NEw : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OneTimeActivities", new[] { "User_Id" });
            DropColumn("dbo.OneTimeActivities", "UserId");
            RenameColumn(table: "dbo.OneTimeActivities", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.OneTimeActivities", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OneTimeActivities", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OneTimeActivities", new[] { "UserId" });
            AlterColumn("dbo.OneTimeActivities", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.OneTimeActivities", name: "UserId", newName: "User_Id");
            AddColumn("dbo.OneTimeActivities", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.OneTimeActivities", "User_Id");
        }
    }
}
