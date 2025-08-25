namespace MyPawDiaryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MedicalRecords", "IsCompleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicalRecords", "IsCompleted", c => c.Boolean(nullable: false));
        }
    }
}
