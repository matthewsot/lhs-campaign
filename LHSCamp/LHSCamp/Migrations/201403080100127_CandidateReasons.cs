namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CandidateReasons : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Reasons", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Reasons");
        }
    }
}
