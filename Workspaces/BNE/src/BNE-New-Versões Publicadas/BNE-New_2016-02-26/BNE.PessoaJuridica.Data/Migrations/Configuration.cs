namespace BNE.PessoaJuridica.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<PessoaJuridicaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PessoaJuridicaContext context)
        {

            //#region Perfil
            //context.Perfil.AddOrUpdate(
            //    n => n.Descricao,
            //    new Perfil { Id = 1, Descricao = "Administrador" },
            //    new Perfil { Id = 2, Descricao = "Master" },
            //    new Perfil { Id = 3, Descricao = "Selecionador" }
            //);
            //#endregion

            //#region TipoTelefone
            //context.TipoTelefone.AddOrUpdate(
            //    n => n.Descricao,
            //    new TipoTelefone { Id = 0, Descricao = "Fixo" },
            //    new TipoTelefone { Id = 1, Descricao = "Celular" },
            //    new TipoTelefone { Id = 2, Descricao = "Comercial" }
            //    );
            //#endregion

        }
    }
}
