namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoSwaggerConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("ApiGateway.SwaggerConfig", "FileName", c => c.String(nullable: false, maxLength: 250, unicode: false));
            AddColumn("ApiGateway.SwaggerConfig", "Authentication_Interface", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("ApiGateway.SwaggerConfig", "Authentication_Interface");
            AddForeignKey("ApiGateway.SwaggerConfig", "Authentication_Interface", "ApiGateway.Authentication", "Interface");
        }
        
        public override void Down()
        {
            DropForeignKey("ApiGateway.SwaggerConfig", "Authentication_Interface", "ApiGateway.Authentication");
            DropIndex("ApiGateway.SwaggerConfig", new[] { "Authentication_Interface" });
            DropColumn("ApiGateway.SwaggerConfig", "Authentication_Interface");
            DropColumn("ApiGateway.SwaggerConfig", "FileName");
        }
    }
}
