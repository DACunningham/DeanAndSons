namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUsrRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Forename", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.AspNetUsers", "About", c => c.String());
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AspNetUsers", "UserType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserType", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropColumn("dbo.AspNetUsers", "About");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Forename");
        }
    }
}
