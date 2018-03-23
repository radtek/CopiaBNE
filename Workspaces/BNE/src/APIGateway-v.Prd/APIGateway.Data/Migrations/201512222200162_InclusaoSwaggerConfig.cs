namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InclusaoSwaggerConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ApiGateway.SwaggerConfig",
                c => new
                    {
                        UrlSuffix = c.String(nullable: false, maxLength: 50, unicode: false),
                        UIUrl = c.String(nullable: false, maxLength: 50, unicode: false),
                        FileUrl = c.String(nullable: false, maxLength: 150, unicode: false),
                        Theme = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.UrlSuffix)
                .ForeignKey("ApiGateway.Api", t => t.UrlSuffix)
                .Index(t => t.UrlSuffix);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ApiGateway.SwaggerConfig", "UrlSuffix", "ApiGateway.Api");
            DropIndex("ApiGateway.SwaggerConfig", new[] { "UrlSuffix" });
            DropTable("ApiGateway.SwaggerConfig");
        }
    }
}
