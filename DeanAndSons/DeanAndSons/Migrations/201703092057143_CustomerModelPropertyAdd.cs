namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerModelPropertyAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PrefPropertyType", c => c.Int());
            AddColumn("dbo.AspNetUsers", "PrefPropertyStyle", c => c.Int());
            AddColumn("dbo.AspNetUsers", "PrefPropertyAge", c => c.Int());
            AddColumn("dbo.AspNetUsers", "PrefNoBedRms", c => c.Int());
            AddColumn("dbo.AspNetUsers", "PrefNoBathRms", c => c.Int());
            AddColumn("dbo.AspNetUsers", "PrefNoSittingRms", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PrefNoSittingRms");
            DropColumn("dbo.AspNetUsers", "PrefNoBathRms");
            DropColumn("dbo.AspNetUsers", "PrefNoBedRms");
            DropColumn("dbo.AspNetUsers", "PrefPropertyAge");
            DropColumn("dbo.AspNetUsers", "PrefPropertyStyle");
            DropColumn("dbo.AspNetUsers", "PrefPropertyType");
        }
    }
}
