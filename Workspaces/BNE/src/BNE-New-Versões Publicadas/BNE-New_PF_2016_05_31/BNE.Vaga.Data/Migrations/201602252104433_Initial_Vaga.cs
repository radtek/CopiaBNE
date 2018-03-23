namespace BNE.Vaga.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Vaga : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "vaga.Beneficio",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        PessoaJuridica = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("vaga.Beneficio");
        }
    }
}
