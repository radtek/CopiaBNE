namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AumentoTamanhoUrlPrefix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ApiGateway.Endpoint", "ApiUrlSuffix", "ApiGateway.Api");
            DropForeignKey("ApiGateway.ApiSistema", "UrlSuffix_Api", "ApiGateway.Api");
            DropIndex("ApiGateway.Endpoint", "UQ_Endpoint");
            DropIndex("ApiGateway.ApiSistema", new[] { "UrlSuffix_Api" });
            DropPrimaryKey("ApiGateway.Api");
            DropPrimaryKey("ApiGateway.ApiSistema");
            AlterColumn("ApiGateway.Api", "UrlSuffix", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("ApiGateway.Endpoint", "ApiUrlSuffix", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("ApiGateway.ApiSistema", "UrlSuffix_Api", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddPrimaryKey("ApiGateway.Api", "UrlSuffix");
            AddPrimaryKey("ApiGateway.ApiSistema", new[] { "UrlSuffix_Api", "Chave_Sistema" });
            CreateIndex("ApiGateway.Endpoint", new[] { "ApiUrlSuffix", "RelativePath", "Method" }, unique: true, name: "UQ_Endpoint");
            CreateIndex("ApiGateway.ApiSistema", "UrlSuffix_Api");
            AddForeignKey("ApiGateway.Endpoint", "ApiUrlSuffix", "ApiGateway.Api", "UrlSuffix");
            AddForeignKey("ApiGateway.ApiSistema", "UrlSuffix_Api", "ApiGateway.Api", "UrlSuffix");
        }
        
        public override void Down()
        {
            DropForeignKey("ApiGateway.ApiSistema", "UrlSuffix_Api", "ApiGateway.Api");
            DropForeignKey("ApiGateway.Endpoint", "ApiUrlSuffix", "ApiGateway.Api");
            DropIndex("ApiGateway.ApiSistema", new[] { "UrlSuffix_Api" });
            DropIndex("ApiGateway.Endpoint", "UQ_Endpoint");
            DropPrimaryKey("ApiGateway.ApiSistema");
            DropPrimaryKey("ApiGateway.Api");
            AlterColumn("ApiGateway.ApiSistema", "UrlSuffix_Api", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("ApiGateway.Endpoint", "ApiUrlSuffix", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("ApiGateway.Api", "UrlSuffix", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddPrimaryKey("ApiGateway.ApiSistema", new[] { "UrlSuffix_Api", "Chave_Sistema" });
            AddPrimaryKey("ApiGateway.Api", "UrlSuffix");
            CreateIndex("ApiGateway.ApiSistema", "UrlSuffix_Api");
            CreateIndex("ApiGateway.Endpoint", new[] { "ApiUrlSuffix", "RelativePath", "Method" }, unique: true, name: "UQ_Endpoint");
            AddForeignKey("ApiGateway.ApiSistema", "UrlSuffix_Api", "ApiGateway.Api", "UrlSuffix");
            AddForeignKey("ApiGateway.Endpoint", "ApiUrlSuffix", "ApiGateway.Api", "UrlSuffix");
        }
    }
}
