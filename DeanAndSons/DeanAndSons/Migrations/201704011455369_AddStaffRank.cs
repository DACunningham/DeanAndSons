namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaffRank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Rank", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Rank");
        }
    }
}
