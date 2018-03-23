using System.Data.Entity.Migrations;

namespace BNE.PessoaJuridica.Data.Migrations
{
    public partial class QuantidadeFuncionarioIP : DbMigration
    {
        public override void Up()
        {
            AddColumn("pessoajuridica.PessoaJuridica", "QuantidadeFuncionario", c => c.Int(false));
            AddColumn("pessoajuridica.PessoaJuridica", "IP", c => c.String(false, 15, unicode: false));
            AddColumn("pessoajuridica.UsuarioPessoaJuridica", "IP", c => c.String(false, 15, unicode: false));
            AddColumn("pessoajuridica.Usuario", "IP", c => c.String(false, 15, unicode: false));
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