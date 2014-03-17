namespace LHSCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignupDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SignupDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SignupDate");
        }
    }
}
