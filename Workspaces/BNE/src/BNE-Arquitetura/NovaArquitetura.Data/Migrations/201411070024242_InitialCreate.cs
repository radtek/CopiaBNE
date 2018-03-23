namespace NovaArquitetura.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aluno",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 500),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAtualizacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disciplina",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 500),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAtualizacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DisciplinaAluno",
                c => new
                    {
                        Disciplina_Id = c.Int(nullable: false),
                        Aluno_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Disciplina_Id, t.Aluno_Id })
                .ForeignKey("dbo.Disciplina", t => t.Disciplina_Id, cascadeDelete: true)
                .ForeignKey("dbo.Aluno", t => t.Aluno_Id, cascadeDelete: true)
                .Index(t => t.Disciplina_Id)
                .Index(t => t.Aluno_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DisciplinaAluno", "Aluno_Id", "dbo.Aluno");
            DropForeignKey("dbo.DisciplinaAluno", "Disciplina_Id", "dbo.Disciplina");
            DropIndex("dbo.DisciplinaAluno", new[] { "Aluno_Id" });
            DropIndex("dbo.DisciplinaAluno", new[] { "Disciplina_Id" });
            DropTable("dbo.DisciplinaAluno");
            DropTable("dbo.Disciplina");
            DropTable("dbo.Aluno");
        }
    }
}
