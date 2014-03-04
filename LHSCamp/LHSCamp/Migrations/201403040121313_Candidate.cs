namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Candidate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Position = c.String(),
                        ProfilePic = c.String(),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            DropColumn("dbo.AspNetUsers", "Position");
            DropColumn("dbo.AspNetUsers", "IsCandidate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsCandidate", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Position", c => c.String());
            DropForeignKey("dbo.Candidates", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Candidates", new[] { "Owner_Id" });
            DropTable("dbo.Candidates");
        }
    }
}
