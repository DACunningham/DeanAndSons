namespace DeanAndSons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConversationInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        SenderID = c.String(maxLength: 128),
                        ReceiverID = c.String(maxLength: 128),
                        LastNewMessage = c.DateTime(nullable: false),
                        LastCheckedSender = c.DateTime(nullable: false),
                        LastCheckedReceiver = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverID)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderID)
                .Index(t => t.SenderID)
                .Index(t => t.ReceiverID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        ConversationID = c.Int(nullable: false),
                        Body = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        AuthorID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Conversations", t => t.ConversationID, cascadeDelete: true)
                .Index(t => t.ConversationID)
                .Index(t => t.AuthorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conversations", "SenderID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "ReceiverID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ConversationID", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "AuthorID", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "AuthorID" });
            DropIndex("dbo.Messages", new[] { "ConversationID" });
            DropIndex("dbo.Conversations", new[] { "ReceiverID" });
            DropIndex("dbo.Conversations", new[] { "SenderID" });
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
        }
    }
}
