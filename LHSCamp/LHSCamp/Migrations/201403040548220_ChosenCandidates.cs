namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChosenCandidates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCandidates",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Candidate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Candidate_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Candidate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.UserCandidates", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.UserCandidates", new[] { "User_Id" });
            DropTable("dbo.UserCandidates");
        }
    }
}
