namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactLatLong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Contacts", "Long", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Long");
            DropColumn("dbo.Contacts", "Lat");
        }
    }
}
