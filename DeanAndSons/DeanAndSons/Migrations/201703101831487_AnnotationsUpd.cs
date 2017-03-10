namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnnotationsUpd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        StaffOwnerID = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.AspNetUsers", t => t.StaffOwnerID, cascadeDelete: true)
                .Index(t => t.StaffOwnerID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        StaffOwnerID = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ServiceID)
                .ForeignKey("dbo.AspNetUsers", t => t.StaffOwnerID, cascadeDelete: false)
                .Index(t => t.StaffOwnerID);
            
            AddColumn("dbo.Contacts", "EventID", c => c.Int());
            AddColumn("dbo.Propertys", "StaffOwnerID", c => c.String(maxLength: 128));
            AddColumn("dbo.Images", "EventID", c => c.Int());
            AddColumn("dbo.Images", "ServiceID", c => c.Int());
            AlterColumn("dbo.Propertys", "Description", c => c.String(nullable: false));
            CreateIndex("dbo.Contacts", "EventID");
            CreateIndex("dbo.Propertys", "StaffOwnerID");
            CreateIndex("dbo.Images", "EventID");
            CreateIndex("dbo.Images", "ServiceID");
            AddForeignKey("dbo.Contacts", "EventID", "dbo.Events", "EventID", cascadeDelete: true);
            AddForeignKey("dbo.Images", "EventID", "dbo.Events", "EventID", cascadeDelete: true);
            AddForeignKey("dbo.Propertys", "StaffOwnerID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Images", "ServiceID", "dbo.Services", "ServiceID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "StaffOwnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.Propertys", "StaffOwnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "StaffOwnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "EventID", "dbo.Events");
            DropForeignKey("dbo.Contacts", "EventID", "dbo.Events");
            DropIndex("dbo.Services", new[] { "StaffOwnerID" });
            DropIndex("dbo.Events", new[] { "StaffOwnerID" });
            DropIndex("dbo.Images", new[] { "ServiceID" });
            DropIndex("dbo.Images", new[] { "EventID" });
            DropIndex("dbo.Propertys", new[] { "StaffOwnerID" });
            DropIndex("dbo.Contacts", new[] { "EventID" });
            AlterColumn("dbo.Propertys", "Description", c => c.String(nullable: false, maxLength: 3000));
            DropColumn("dbo.Images", "ServiceID");
            DropColumn("dbo.Images", "EventID");
            DropColumn("dbo.Propertys", "StaffOwnerID");
            DropColumn("dbo.Contacts", "EventID");
            DropTable("dbo.Services");
            DropTable("dbo.Events");
        }
    }
}
