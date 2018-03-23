namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UQNomesistemaCliente : DbMigration
    {
        public override void Up()
        {
            CreateIndex("ApiGateway.SistemaCliente", "Nome", unique: true, name: "UQ_SistemaClienteNome");
        }
        
        public override void Down()
        {
            DropIndex("ApiGateway.SistemaCliente", "UQ_SistemaClienteNome");
        }
    }
}
