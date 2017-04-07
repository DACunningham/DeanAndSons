namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SavedProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PropertyCustomers",
                c => new
                    {
                        Property_PropertyID = c.Int(nullable: false),
                        Customer_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Property_PropertyID, t.Customer_Id })
                .ForeignKey("dbo.Propertys", t => t.Property_PropertyID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Property_PropertyID)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertyCustomers", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PropertyCustomers", "Property_PropertyID", "dbo.Propertys");
            DropIndex("dbo.PropertyCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.PropertyCustomers", new[] { "Property_PropertyID" });
            DropTable("dbo.PropertyCustomers");
        }
    }
}
