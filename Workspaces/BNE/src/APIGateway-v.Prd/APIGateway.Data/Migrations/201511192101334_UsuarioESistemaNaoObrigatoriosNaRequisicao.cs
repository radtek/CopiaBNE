namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioESistemaNaoObrigatoriosNaRequisicao : DbMigration
    {
        public override void Up()
        {
            DropIndex("ApiGateway.Requisicao", new[] { "SistemaCliente_Chave" });
            DropIndex("ApiGateway.Requisicao", new[] { "Usuario_Id" });
            AlterColumn("ApiGateway.Requisicao", "SistemaCliente_Chave", c => c.Guid());
            AlterColumn("ApiGateway.Requisicao", "Usuario_Id", c => c.Int());
            CreateIndex("ApiGateway.Requisicao", "SistemaCliente_Chave");
            CreateIndex("ApiGateway.Requisicao", "Usuario_Id");
        }
        
        public override void Down()
        {
            DropIndex("ApiGateway.Requisicao", new[] { "Usuario_Id" });
            DropIndex("ApiGateway.Requisicao", new[] { "SistemaCliente_Chave" });
            AlterColumn("ApiGateway.Requisicao", "Usuario_Id", c => c.Int(nullable: false));
            AlterColumn("ApiGateway.Requisicao", "SistemaCliente_Chave", c => c.Guid(nullable: false));
            CreateIndex("ApiGateway.Requisicao", "Usuario_Id");
            CreateIndex("ApiGateway.Requisicao", "SistemaCliente_Chave");
        }
    }
}
