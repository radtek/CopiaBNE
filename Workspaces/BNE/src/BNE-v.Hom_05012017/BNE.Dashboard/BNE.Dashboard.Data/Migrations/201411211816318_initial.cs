namespace BNE.Dashboard.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dashboard.GoogleAnalyticsSites",
                c => new
                    {
                        GoogleAnalyticsSitesId = c.Int(nullable: false, identity: true),
                        Site = c.String(),
                        ViewID = c.String(),
                        ActiveUsers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GoogleAnalyticsSitesId);
            
            CreateTable(
                "dashboard.MessageQueue",
                c => new
                    {
                        MessageQueueId = c.Int(nullable: false, identity: true),
                        MessageQueueName = c.String(),
                        MessageQueueServer = c.String(),
                        MaximumMessageCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageQueueId);
            
            CreateTable(
                "dashboard.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dashboard.Watcher",
                c => new
                    {
                        WatcherId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MessageQueue_MessageQueueId = c.Int(),
                        SiteResponse_SiteResponseId = c.Int(),
                        Status_StatusId = c.Int(),
                        WindowsService_WindowsServiceId = c.Int(),
                    })
                .PrimaryKey(t => t.WatcherId)
                .ForeignKey("dashboard.MessageQueue", t => t.MessageQueue_MessageQueueId)
                .ForeignKey("dashboard.SiteResponse", t => t.SiteResponse_SiteResponseId)
                .ForeignKey("dashboard.Status", t => t.Status_StatusId)
                .ForeignKey("dashboard.WindowsService", t => t.WindowsService_WindowsServiceId)
                .Index(t => t.MessageQueue_MessageQueueId)
                .Index(t => t.SiteResponse_SiteResponseId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.WindowsService_WindowsServiceId);
            
            CreateTable(
                "dashboard.SiteResponse",
                c => new
                    {
                        SiteResponseId = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.SiteResponseId);
            
            CreateTable(
                "dashboard.WindowsService",
                c => new
                    {
                        WindowsServiceId = c.Int(nullable: false, identity: true),
                        WindowsServiceName = c.String(),
                    })
                .PrimaryKey(t => t.WindowsServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dashboard.Watcher", "WindowsService_WindowsServiceId", "dashboard.WindowsService");
            DropForeignKey("dashboard.Watcher", "Status_StatusId", "dashboard.Status");
            DropForeignKey("dashboard.Watcher", "SiteResponse_SiteResponseId", "dashboard.SiteResponse");
            DropForeignKey("dashboard.Watcher", "MessageQueue_MessageQueueId", "dashboard.MessageQueue");
            DropIndex("dashboard.Watcher", new[] { "WindowsService_WindowsServiceId" });
            DropIndex("dashboard.Watcher", new[] { "Status_StatusId" });
            DropIndex("dashboard.Watcher", new[] { "SiteResponse_SiteResponseId" });
            DropIndex("dashboard.Watcher", new[] { "MessageQueue_MessageQueueId" });
            DropTable("dashboard.WindowsService");
            DropTable("dashboard.SiteResponse");
            DropTable("dashboard.Watcher");
            DropTable("dashboard.Status");
            DropTable("dashboard.MessageQueue");
            DropTable("dashboard.GoogleAnalyticsSites");
        }
    }
}
