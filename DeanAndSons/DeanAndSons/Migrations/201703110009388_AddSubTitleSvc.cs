namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubTitleSvc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "SubTitle", c => c.String(nullable: false, maxLength: 65));
            AlterColumn("dbo.Services", "Title", c => c.String(nullable: false, maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "Title", c => c.String(nullable: false, maxLength: 75));
            DropColumn("dbo.Services", "SubTitle");
        }
    }
}
