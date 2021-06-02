namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDomainClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.chat_client",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        last_seen = c.DateTime(),
                        date_added = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.chat_file",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        size = c.Double(nullable: false),
                        location = c.String(),
                        dateadded = c.DateTime(nullable: false),
                        receiver_Id = c.String(maxLength: 128),
                        sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.chat_client", t => t.receiver_Id)
                .ForeignKey("dbo.chat_client", t => t.sender_Id)
                .Index(t => t.receiver_Id)
                .Index(t => t.sender_Id);
            
            CreateTable(
                "dbo.chat_message",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        message_text = c.String(),
                        date_added = c.DateTime(nullable: false),
                        receiver_Id = c.String(maxLength: 128),
                        sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.chat_client", t => t.receiver_Id)
                .ForeignKey("dbo.chat_client", t => t.sender_Id)
                .Index(t => t.receiver_Id)
                .Index(t => t.sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.chat_message", "sender_Id", "dbo.chat_client");
            DropForeignKey("dbo.chat_message", "receiver_Id", "dbo.chat_client");
            DropForeignKey("dbo.chat_file", "sender_Id", "dbo.chat_client");
            DropForeignKey("dbo.chat_file", "receiver_Id", "dbo.chat_client");
            DropIndex("dbo.chat_message", new[] { "sender_Id" });
            DropIndex("dbo.chat_message", new[] { "receiver_Id" });
            DropIndex("dbo.chat_file", new[] { "sender_Id" });
            DropIndex("dbo.chat_file", new[] { "receiver_Id" });
            DropTable("dbo.chat_message");
            DropTable("dbo.chat_file");
            DropTable("dbo.chat_client");
        }
    }
}
