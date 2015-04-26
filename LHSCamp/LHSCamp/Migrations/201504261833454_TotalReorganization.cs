namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalReorganization : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Candidates", newName: "AspNetUsers");
            DropForeignKey("dbo.Candidates", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCandidates", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExternalLinks", "Candidate_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Candidates", new[] { "Owner_Id" });
            DropIndex("dbo.UserCandidates", new[] { "User_Id" });
            DropIndex("dbo.UserCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ExternalLinks", new[] { "Candidate_Id" });
            DropPrimaryKey("dbo.Candidates");
            CreateTable(
                "dbo.ExternalLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        Link = c.String(),
                        Candidate_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Candidate_Id)
                .Index(t => t.Candidate_Id);
            
            AddColumn("dbo.AspNetUsers", "GraduationYear", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ProfilePicture", c => c.String());
            AddColumn("dbo.AspNetUsers", "Platform", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Email", c => c.String());
            AddColumn("dbo.AspNetUsers", "PasswordHash", c => c.String());
            AddColumn("dbo.AspNetUsers", "SecurityStamp", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsConfirmed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.AspNetUsers", "ProfilePic");
            DropColumn("dbo.AspNetUsers", "Reasons");
            DropColumn("dbo.AspNetUsers", "Facebook");
            DropColumn("dbo.AspNetUsers", "Owner_Id");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserCandidates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserCandidates",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Candidate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Candidate_Id });
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Year = c.Int(nullable: false),
                        SignupDate = c.DateTime(),
                        UserName = c.String(nullable: false),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Owner_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Facebook", c => c.String());
            AddColumn("dbo.AspNetUsers", "Reasons", c => c.String());
            AddColumn("dbo.AspNetUsers", "ProfilePic", c => c.String());
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExternalLinks", "Candidate_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ExternalLinks", new[] { "Candidate_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropPrimaryKey("dbo.AspNetUsers");
            AlterColumn("dbo.AspNetUsers", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AspNetUsers", "IsConfirmed");
            DropColumn("dbo.AspNetUsers", "SecurityStamp");
            DropColumn("dbo.AspNetUsers", "PasswordHash");
            DropColumn("dbo.AspNetUsers", "Email");
            DropColumn("dbo.AspNetUsers", "UserName");
            DropColumn("dbo.AspNetUsers", "Platform");
            DropColumn("dbo.AspNetUsers", "ProfilePicture");
            DropColumn("dbo.AspNetUsers", "GraduationYear");
            DropTable("dbo.ExternalLinks");
            AddPrimaryKey("dbo.Candidates", "Id");
            CreateIndex("dbo.ExternalLinks", "Candidate_Id");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.UserCandidates", "Candidate_Id");
            CreateIndex("dbo.UserCandidates", "User_Id");
            CreateIndex("dbo.Candidates", "Owner_Id");
            AddForeignKey("dbo.ExternalLinks", "Candidate_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserCandidates", "Candidate_Id", "dbo.Candidates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserCandidates", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Candidates", "Owner_Id", "dbo.AspNetUsers", "Id");
            RenameTable(name: "dbo.AspNetUsers", newName: "Candidates");
        }
    }
}
