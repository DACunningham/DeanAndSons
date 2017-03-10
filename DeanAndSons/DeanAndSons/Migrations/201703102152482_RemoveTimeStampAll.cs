namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTimeStampAll : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "PropertyNo", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Contacts", "Street", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.Contacts", "Town", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.Contacts", "PostCode", c => c.String(nullable: false, maxLength: 9));
            DropColumn("dbo.Contacts", "RowVersion");
            DropColumn("dbo.Propertys", "RowVersion");
            DropColumn("dbo.Images", "RowVersion");
            DropColumn("dbo.Events", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Images", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Propertys", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Contacts", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.Contacts", "PostCode", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Town", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Street", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "PropertyNo", c => c.String(nullable: false));
        }
    }
}
