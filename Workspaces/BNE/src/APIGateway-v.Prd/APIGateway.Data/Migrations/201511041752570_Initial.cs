namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ApiGateway.Api",
                c => new
                    {
                        UrlSuffix = c.String(nullable: false, maxLength: 20, unicode: false),
                        BaseUrl = c.String(nullable: false, maxLength: 150, unicode: false),
                        AuthenticationType_Interface = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.UrlSuffix)
                .ForeignKey("ApiGateway.Authentication", t => t.AuthenticationType_Interface)
                .Index(t => t.AuthenticationType_Interface);
            
            CreateTable(
                "ApiGateway.Authentication",
                c => new
                    {
                        Interface = c.String(nullable: false, maxLength: 20, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Interface);
            
            CreateTable(
                "ApiGateway.OAuthConfig",
                c => new
                    {
                        Interface = c.String(nullable: false, maxLength: 20, unicode: false),
                        TokenEndpoint = c.String(nullable: false, maxLength: 50, unicode: false),
                        AuthenticationEndpoint = c.String(nullable: false, maxLength: 150, unicode: false),
                        SecretKey = c.String(nullable: false, maxLength: 16, unicode: false),
                        Expiration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Interface)
                .ForeignKey("ApiGateway.Authentication", t => t.Interface)
                .Index(t => t.Interface);
            
            CreateTable(
                "ApiGateway.Endpoint",
                c => new
                    {
                        Id = c.Short(nullable: false, identity: true),
                        RelativePath = c.String(nullable: false, maxLength: 200, unicode: false),
                        Method = c.String(nullable: false, maxLength: 10, unicode: false),
                        LogSucesso = c.Boolean(nullable: false),
                        LogErro = c.Boolean(nullable: false),
                        LogResponse = c.Boolean(nullable: false),
                        ApiUrlSuffix = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ApiGateway.Api", t => t.ApiUrlSuffix)
                .Index(t => new { t.ApiUrlSuffix, t.RelativePath, t.Method }, unique: true, name: "UQ_Endpoint");
            
            CreateTable(
                "ApiGateway.SistemaCliente",
                c => new
                    {
                        Chave = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Chave);
            
            CreateTable(
                "ApiGateway.Authorization",
                c => new
                    {
                        EndpointId = c.Short(nullable: false),
                        Perfil = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EndpointId, t.Perfil })
                .ForeignKey("ApiGateway.Endpoint", t => t.EndpointId)
                .Index(t => t.EndpointId);
            
            CreateTable(
                "ApiGateway.Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(maxLength: 8000, unicode: false),
                        Value = c.String(maxLength: 8000, unicode: false),
                        UsuarioSistemaCliente_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ApiGateway.Usuario", t => t.UsuarioSistemaCliente_Id)
                .Index(t => t.UsuarioSistemaCliente_Id);
            
            CreateTable(
                "ApiGateway.Requisicao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TempoExecucao = c.Double(nullable: false),
                        ResponseStatusCode = c.Int(nullable: false),
                        DataRequisicao = c.DateTime(nullable: false),
                        Request = c.String(nullable: false, maxLength: 8000, unicode: false),
                        RequestContent = c.String(maxLength: 8000, unicode: false),
                        Perfil = c.String(nullable: false, maxLength: 50, unicode: false),
                        Response = c.String(maxLength: 8000, unicode: false),
                        ResponseContent = c.String(maxLength: 8000, unicode: false),
                        Endpoint_Id = c.Short(nullable: false),
                        SistemaCliente_Chave = c.Guid(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ApiGateway.Endpoint", t => t.Endpoint_Id)
                .ForeignKey("ApiGateway.SistemaCliente", t => t.SistemaCliente_Chave)
                .ForeignKey("ApiGateway.Usuario", t => t.Usuario_Id)
                .Index(t => t.Endpoint_Id)
                .Index(t => t.SistemaCliente_Chave)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "ApiGateway.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CPF = c.Decimal(precision: 11, scale: 0),
                        DataNascimento = c.DateTime(storeType: "date"),
                        CNPJ = c.Decimal(precision: 14, scale: 0),
                        Perfil = c.String(maxLength: 50, unicode: false),
                        SistemaCliente_Chave = c.Guid(),
                        Type = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ApiGateway.SistemaCliente", t => t.SistemaCliente_Chave)
                .Index(t => t.SistemaCliente_Chave);
            
            CreateTable(
                "ApiGateway.ApiSistema",
                c => new
                    {
                        UrlSuffix_Api = c.String(nullable: false, maxLength: 20, unicode: false),
                        Chave_Sistema = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UrlSuffix_Api, t.Chave_Sistema })
                .ForeignKey("ApiGateway.Api", t => t.UrlSuffix_Api)
                .ForeignKey("ApiGateway.SistemaCliente", t => t.Chave_Sistema)
                .Index(t => t.UrlSuffix_Api)
                .Index(t => t.Chave_Sistema);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ApiGateway.Requisicao", "Usuario_Id", "ApiGateway.Usuario");
            DropForeignKey("ApiGateway.Usuario", "SistemaCliente_Chave", "ApiGateway.SistemaCliente");
            DropForeignKey("ApiGateway.Header", "UsuarioSistemaCliente_Id", "ApiGateway.Usuario");
            DropForeignKey("ApiGateway.Requisicao", "SistemaCliente_Chave", "ApiGateway.SistemaCliente");
            DropForeignKey("ApiGateway.Requisicao", "Endpoint_Id", "ApiGateway.Endpoint");
            DropForeignKey("ApiGateway.Authorization", "EndpointId", "ApiGateway.Endpoint");
            DropForeignKey("ApiGateway.ApiSistema", "Chave_Sistema", "ApiGateway.SistemaCliente");
            DropForeignKey("ApiGateway.ApiSistema", "UrlSuffix_Api", "ApiGateway.Api");
            DropForeignKey("ApiGateway.Endpoint", "ApiUrlSuffix", "ApiGateway.Api");
            DropForeignKey("ApiGateway.Api", "AuthenticationType_Interface", "ApiGateway.Authentication");
            DropForeignKey("ApiGateway.OAuthConfig", "Interface", "ApiGateway.Authentication");
            DropIndex("ApiGateway.ApiSistema", new[] { "Chave_Sistema" });
            DropIndex("ApiGateway.ApiSistema", new[] { "UrlSuffix_Api" });
            DropIndex("ApiGateway.Usuario", new[] { "SistemaCliente_Chave" });
            DropIndex("ApiGateway.Requisicao", new[] { "Usuario_Id" });
            DropIndex("ApiGateway.Requisicao", new[] { "SistemaCliente_Chave" });
            DropIndex("ApiGateway.Requisicao", new[] { "Endpoint_Id" });
            DropIndex("ApiGateway.Header", new[] { "UsuarioSistemaCliente_Id" });
            DropIndex("ApiGateway.Authorization", new[] { "EndpointId" });
            DropIndex("ApiGateway.Endpoint", "UQ_Endpoint");
            DropIndex("ApiGateway.OAuthConfig", new[] { "Interface" });
            DropIndex("ApiGateway.Api", new[] { "AuthenticationType_Interface" });
            DropTable("ApiGateway.ApiSistema");
            DropTable("ApiGateway.Usuario");
            DropTable("ApiGateway.Requisicao");
            DropTable("ApiGateway.Header");
            DropTable("ApiGateway.Authorization");
            DropTable("ApiGateway.SistemaCliente");
            DropTable("ApiGateway.Endpoint");
            DropTable("ApiGateway.OAuthConfig");
            DropTable("ApiGateway.Authentication");
            DropTable("ApiGateway.Api");
        }
    }
}
