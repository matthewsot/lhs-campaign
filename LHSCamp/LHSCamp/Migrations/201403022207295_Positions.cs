namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Positions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Position", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsCandidate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsCandidate");
            DropColumn("dbo.AspNetUsers", "Position");
        }
    }
}
