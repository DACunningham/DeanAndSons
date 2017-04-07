namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SavedSearch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedSearches",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        Radius = c.Int(nullable: false),
                        MinPrice = c.Int(nullable: false),
                        MaxPrice = c.Int(nullable: false),
                        Beds = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        CategorySort = c.Int(nullable: false),
                        OrderSort = c.Int(nullable: false),
                        CustomerID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavedSearches", "CustomerID", "dbo.AspNetUsers");
            DropIndex("dbo.SavedSearches", new[] { "CustomerID" });
            DropTable("dbo.SavedSearches");
        }
    }
}
