namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyNoToStr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "PropertyNo", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Street", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Town", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "PostCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "PostCode", c => c.String());
            AlterColumn("dbo.Contacts", "Town", c => c.String());
            AlterColumn("dbo.Contacts", "Street", c => c.String());
            AlterColumn("dbo.Contacts", "PropertyNo", c => c.Int(nullable: false));
        }
    }
}
