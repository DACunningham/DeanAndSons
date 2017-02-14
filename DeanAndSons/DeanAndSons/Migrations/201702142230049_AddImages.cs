namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        Type = c.Int(nullable: false),
                        PropertyID = c.Int(),
                        UserID = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ImageID)
                .ForeignKey("dbo.Properties", t => t.PropertyID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.PropertyID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "PropertyID", "dbo.Properties");
            DropIndex("dbo.Images", new[] { "UserID" });
            DropIndex("dbo.Images", new[] { "PropertyID" });
            DropTable("dbo.Images");
        }
    }
}
