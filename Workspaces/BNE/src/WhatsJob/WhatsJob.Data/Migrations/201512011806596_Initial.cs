namespace WhatsJob.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "WhatsJob.Channel",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 150, unicode: false),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        NickName = c.String(nullable: false, maxLength: 100, unicode: false),
                        NextChallenge = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "WhatsJob.ChannelLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Text = c.String(maxLength: 8000, unicode: false),
                        FaultType = c.Int(nullable: false),
                        Channel_Number = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("WhatsJob.Channel", t => t.Channel_Number, cascadeDelete: true)
                .Index(t => t.Channel_Number);
            
            CreateTable(
                "WhatsJob.Contact",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 150, unicode: false),
                        From = c.String(nullable: false, maxLength: 150, unicode: false),
                        NickName = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "WhatsJob.Message",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 150, unicode: false),
                        TextMessage = c.String(maxLength: 8000, unicode: false),
                        Received = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ReceivedByServer = c.DateTime(),
                        ReceivedByClient = c.DateTime(),
                        ReadByClient = c.DateTime(),
                        Replied = c.Boolean(nullable: false),
                        SentToServer = c.Boolean(nullable: false),
                        Contact_Number = c.String(nullable: false, maxLength: 150, unicode: false),
                        WhatsChannel_Number = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("WhatsJob.Contact", t => t.Contact_Number, cascadeDelete: true)
                .ForeignKey("WhatsJob.Channel", t => t.WhatsChannel_Number, cascadeDelete: true)
                .Index(t => t.Contact_Number)
                .Index(t => t.WhatsChannel_Number);
            
        }
        
        public override void Down()
        {
            DropForeignKey("WhatsJob.Message", "WhatsChannel_Number", "WhatsJob.Channel");
            DropForeignKey("WhatsJob.Message", "Contact_Number", "WhatsJob.Contact");
            DropForeignKey("WhatsJob.ChannelLog", "Channel_Number", "WhatsJob.Channel");
            DropIndex("WhatsJob.Message", new[] { "WhatsChannel_Number" });
            DropIndex("WhatsJob.Message", new[] { "Contact_Number" });
            DropIndex("WhatsJob.ChannelLog", new[] { "Channel_Number" });
            DropTable("WhatsJob.Message");
            DropTable("WhatsJob.Contact");
            DropTable("WhatsJob.ChannelLog");
            DropTable("WhatsJob.Channel");
        }
    }
}
