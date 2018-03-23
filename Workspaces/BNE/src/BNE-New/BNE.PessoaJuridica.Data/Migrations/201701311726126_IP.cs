using System.Data.Entity.Migrations;

namespace BNE.PessoaJuridica.Data.Migrations
{
    public partial class IP : DbMigration
    {
        public override void Up()
        {
            AlterColumn("pessoajuridica.PessoaJuridica", "IP", c => c.String(false, 40, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("pessoajuridica.PessoaJuridica", "IP", c => c.String(false, 15, unicode: false));
        }
    }
}