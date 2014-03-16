namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoverPhotos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "CoverPhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "CoverPhoto");
        }
    }
}
