namespace MyPawDiaryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyToMedicalRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalRecords", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MedicalRecords", "Notes");
        }
    }
}
