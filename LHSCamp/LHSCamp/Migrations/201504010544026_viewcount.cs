namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class viewcount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "ViewCount", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "ViewCount");
        }
    }
}
