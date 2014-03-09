namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialLinks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Facebook", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Facebook");
        }
    }
}
