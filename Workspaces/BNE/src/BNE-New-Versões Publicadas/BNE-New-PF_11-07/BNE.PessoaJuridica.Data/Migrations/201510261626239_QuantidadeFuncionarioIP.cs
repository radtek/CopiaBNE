namespace BNE.PessoaJuridica.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuantidadeFuncionarioIP : DbMigration
    {
        public override void Up()
        {
            AddColumn("pessoajuridica.PessoaJuridica", "QuantidadeFuncionario", c => c.Int(nullable: false));
            AddColumn("pessoajuridica.PessoaJuridica", "IP", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("pessoajuridica.UsuarioPessoaJuridica", "IP", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("pessoajuridica.Usuario", "IP", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("pessoajuridica.Usuario", "IP");
            DropColumn("pessoajuridica.UsuarioPessoaJuridica", "IP");
            DropColumn("pessoajuridica.PessoaJuridica", "IP");
            DropColumn("pessoajuridica.PessoaJuridica", "QuantidadeFuncionario");
        }
    }
}
