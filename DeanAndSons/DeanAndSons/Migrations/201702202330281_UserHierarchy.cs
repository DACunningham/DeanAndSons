namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserHierarchy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SuperiorID", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "SuperiorID");
            AddForeignKey("dbo.AspNetUsers", "SuperiorID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "SuperiorID", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "SuperiorID" });
            DropColumn("dbo.AspNetUsers", "SuperiorID");
        }
    }
}
