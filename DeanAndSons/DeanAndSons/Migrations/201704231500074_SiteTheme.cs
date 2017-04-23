namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteTheme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SiteTheme", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SiteTheme");
        }
    }
}
