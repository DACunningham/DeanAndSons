namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTimeStampSvcs : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
    }
}
