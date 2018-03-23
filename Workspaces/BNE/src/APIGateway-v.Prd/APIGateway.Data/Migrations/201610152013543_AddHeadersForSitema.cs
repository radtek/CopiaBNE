namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeadersForSitema : DbMigration
    {
        public override void Up()
        {
            AddColumn("ApiGateway.Header", "SistemaCliente_Chave", c => c.Guid());
            CreateIndex("ApiGateway.Header", "SistemaCliente_Chave");
            AddForeignKey("ApiGateway.Header", "SistemaCliente_Chave", "ApiGateway.SistemaCliente", "Chave");
        }
        
        public override void Down()
        {
            DropForeignKey("ApiGateway.Header", "SistemaCliente_Chave", "ApiGateway.SistemaCliente");
            DropIndex("ApiGateway.Header", new[] { "SistemaCliente_Chave" });
            DropColumn("ApiGateway.Header", "SistemaCliente_Chave");
        }
    }
}
