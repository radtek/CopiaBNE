namespace BNE.ValidaCelular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "validacelular.TAB_Codigo_Confirmacao_Celular",
                c => new
                    {
                        Idf_Codigo_Confirmacao_Celular = c.Int(nullable: false, identity: true),
                        Dta_Criacao = c.DateTime(nullable: false),
                        Dta_Utilizacao = c.DateTime(),
                        Cod_Confirmacao = c.String(nullable: false, maxLength: 10),
                        Num_DDD_Celular = c.String(nullable: false, maxLength: 2),
                        Num_Celular = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Idf_Codigo_Confirmacao_Celular);
            
        }
        
        public override void Down()
        {
            DropTable("validacelular.TAB_Codigo_Confirmacao_Celular");
        }
    }
}
