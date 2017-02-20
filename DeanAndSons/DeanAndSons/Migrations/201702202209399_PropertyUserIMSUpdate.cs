namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyUserIMSUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contacts", "LastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contacts", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Contacts", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Propertys", "SaleState", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "Style", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "NoBedRms", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "NoBathRms", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "NoSittingRms", c => c.Int(nullable: false));
            AddColumn("dbo.Propertys", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Propertys", "LastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Propertys", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Propertys", "BuyerID", c => c.String(maxLength: 128));
            AddColumn("dbo.Propertys", "SellerID", c => c.String(maxLength: 128));
            AddColumn("dbo.Propertys", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Images", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "LastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Images", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.AspNetUsers", "UserNameDisp", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "BudgetLower", c => c.Int());
            AddColumn("dbo.AspNetUsers", "BudgetHigher", c => c.Int());
            AddColumn("dbo.AspNetUsers", "UserType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Deleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Propertys", "Title", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.Propertys", "Description", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.Images", "Location", c => c.String(nullable: false));
            CreateIndex("dbo.Propertys", "BuyerID");
            CreateIndex("dbo.Propertys", "SellerID");
            AddForeignKey("dbo.Propertys", "BuyerID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Propertys", "SellerID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Propertys", "SellerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Propertys", "BuyerID", "dbo.AspNetUsers");
            DropIndex("dbo.Propertys", new[] { "SellerID" });
            DropIndex("dbo.Propertys", new[] { "BuyerID" });
            AlterColumn("dbo.Images", "Location", c => c.String());
            AlterColumn("dbo.Propertys", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Propertys", "Title", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "Deleted");
            DropColumn("dbo.AspNetUsers", "UserType");
            DropColumn("dbo.AspNetUsers", "BudgetHigher");
            DropColumn("dbo.AspNetUsers", "BudgetLower");
            DropColumn("dbo.AspNetUsers", "UserNameDisp");
            DropColumn("dbo.Images", "RowVersion");
            DropColumn("dbo.Images", "Deleted");
            DropColumn("dbo.Images", "LastModified");
            DropColumn("dbo.Images", "Created");
            DropColumn("dbo.Propertys", "RowVersion");
            DropColumn("dbo.Propertys", "SellerID");
            DropColumn("dbo.Propertys", "BuyerID");
            DropColumn("dbo.Propertys", "Deleted");
            DropColumn("dbo.Propertys", "LastModified");
            DropColumn("dbo.Propertys", "Created");
            DropColumn("dbo.Propertys", "NoSittingRms");
            DropColumn("dbo.Propertys", "NoBathRms");
            DropColumn("dbo.Propertys", "NoBedRms");
            DropColumn("dbo.Propertys", "Age");
            DropColumn("dbo.Propertys", "Style");
            DropColumn("dbo.Propertys", "SaleState");
            DropColumn("dbo.Contacts", "RowVersion");
            DropColumn("dbo.Contacts", "Deleted");
            DropColumn("dbo.Contacts", "LastModified");
            DropColumn("dbo.Contacts", "Created");
        }
    }
}
