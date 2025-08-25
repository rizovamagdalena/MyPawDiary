namespace MyPawDiaryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.Activities", "PetId", "dbo.Pets");
            DropForeignKey("dbo.DailyActivityCompletions", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.MedicalRecords", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.DailyActivityCompletions", "PetId", "dbo.Pets");
            DropIndex("dbo.Activities", new[] { "PetId" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropIndex("dbo.DailyActivityCompletions", new[] { "ActivityTypeId" });
            DropIndex("dbo.MedicalRecords", new[] { "ActivityTypeId" });
            CreateTable(
                "dbo.DailyActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PetId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PetId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OneTimeActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PetId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Notes = c.String(),
                        DateAndTime = c.DateTime(nullable: false),
                        isCompleted = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pets", t => t.PetId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.PetId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.DailyActivityCompletions", "ActivityId", c => c.Int(nullable: false));
            AddColumn("dbo.MedicalRecords", "ActivityId", c => c.Int(nullable: false));
            CreateIndex("dbo.DailyActivityCompletions", "ActivityId");
            CreateIndex("dbo.MedicalRecords", "ActivityId");
            AddForeignKey("dbo.DailyActivityCompletions", "ActivityId", "dbo.DailyActivities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MedicalRecords", "ActivityId", "dbo.OneTimeActivities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DailyActivityCompletions", "PetId", "dbo.Pets", "Id");
            DropColumn("dbo.DailyActivityCompletions", "ActivityTypeId");
            DropColumn("dbo.DailyActivityCompletions", "IsCompleted");
            DropColumn("dbo.MedicalRecords", "ActivityTypeId");
            DropTable("dbo.Activities");
            DropTable("dbo.ActivityTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                        IsRecurringDaily = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PetId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Notes = c.String(),
                        Location = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MedicalRecords", "ActivityTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.DailyActivityCompletions", "IsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DailyActivityCompletions", "ActivityTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DailyActivityCompletions", "PetId", "dbo.Pets");
            DropForeignKey("dbo.MedicalRecords", "ActivityId", "dbo.OneTimeActivities");
            DropForeignKey("dbo.OneTimeActivities", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OneTimeActivities", "PetId", "dbo.Pets");
            DropForeignKey("dbo.DailyActivities", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DailyActivities", "PetId", "dbo.Pets");
            DropForeignKey("dbo.DailyActivityCompletions", "ActivityId", "dbo.DailyActivities");
            DropIndex("dbo.OneTimeActivities", new[] { "User_Id" });
            DropIndex("dbo.OneTimeActivities", new[] { "PetId" });
            DropIndex("dbo.MedicalRecords", new[] { "ActivityId" });
            DropIndex("dbo.DailyActivityCompletions", new[] { "ActivityId" });
            DropIndex("dbo.DailyActivities", new[] { "UserId" });
            DropIndex("dbo.DailyActivities", new[] { "PetId" });
            DropColumn("dbo.MedicalRecords", "ActivityId");
            DropColumn("dbo.DailyActivityCompletions", "ActivityId");
            DropTable("dbo.OneTimeActivities");
            DropTable("dbo.DailyActivities");
            CreateIndex("dbo.MedicalRecords", "ActivityTypeId");
            CreateIndex("dbo.DailyActivityCompletions", "ActivityTypeId");
            CreateIndex("dbo.Activities", "ActivityTypeId");
            CreateIndex("dbo.Activities", "PetId");
            AddForeignKey("dbo.DailyActivityCompletions", "PetId", "dbo.Pets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MedicalRecords", "ActivityTypeId", "dbo.ActivityTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DailyActivityCompletions", "ActivityTypeId", "dbo.ActivityTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "PetId", "dbo.Pets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes", "Id", cascadeDelete: true);
        }
    }
}
