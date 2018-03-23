namespace BNE.PessoaJuridica.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class endereco : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("pessoajuridica.PessoaJuridica", "IdEndereco", "pessoajuridica.Endereco");
            DropIndex("pessoajuridica.PessoaJuridica", new[] { "IdEndereco" });
            AlterColumn("pessoajuridica.PessoaJuridica", "RazaoSocial", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("pessoajuridica.PessoaJuridica", "IdEndereco", c => c.Long());
            CreateIndex("pessoajuridica.PessoaJuridica", "IdEndereco");
            AddForeignKey("pessoajuridica.PessoaJuridica", "IdEndereco", "pessoajuridica.Endereco", "IdEndereco");
        }
        
        public override void Down()
        {
            DropForeignKey("pessoajuridica.PessoaJuridica", "IdEndereco", "pessoajuridica.Endereco");
            DropIndex("pessoajuridica.PessoaJuridica", new[] { "IdEndereco" });
            AlterColumn("pessoajuridica.PessoaJuridica", "IdEndereco", c => c.Long(nullable: false));
            AlterColumn("pessoajuridica.PessoaJuridica", "RazaoSocial", c => c.String(nullable: false, maxLength: 120, unicode: false));
            CreateIndex("pessoajuridica.PessoaJuridica", "IdEndereco");
            AddForeignKey("pessoajuridica.PessoaJuridica", "IdEndereco", "pessoajuridica.Endereco", "IdEndereco", cascadeDelete: true);
        }
    }
}
