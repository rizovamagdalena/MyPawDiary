namespace MyPawDiaryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicturePath = c.String(),
                        Caption = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PostLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowerId = c.String(maxLength: 128),
                        FollowingId = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowingId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowingId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowingId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostLikes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostLikes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.Follows", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Follows", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Follows", new[] { "FollowingId" });
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropIndex("dbo.PostLikes", new[] { "PostId" });
            DropIndex("dbo.PostLikes", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Follows");
            DropTable("dbo.PostLikes");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
