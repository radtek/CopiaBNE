using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BNE.PessoaJuridica.Data
{
    public class PessoaJuridicaContext : DbContext
    {
        public PessoaJuridicaContext()
            : base("name=PessoaJuridica")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Model.PessoaJuridica> PessoaJuridica { get; set; }
        public DbSet<Model.Telefone> Telefone { get; set; }
        public DbSet<Model.Usuario> UsuarioPessoaJuridica { get; set; }
        public DbSet<Model.UsuarioPessoaJuridica> UsuarioPessoaJuridicaPerfil { get; set; }
        public DbSet<Model.Perfil> Perfil { get; set; }
        public DbSet<Model.NaturezaJuridica> NaturezaJuridica { get; set; }
        public DbSet<Model.CNAE> CNAE { get; set; }
        public DbSet<Model.Endereco> Endereco { get; set; }
        public DbSet<Model.Email> Email { get; set; }
        public DbSet<Model.UsuarioAdicional> UsuarioAdicional { get; set; }
        public DbSet<Model.Parametro> Parametro { get; set; }

        public DbSet<Global.Model.Bairro> Bairro { get; set; }
        public DbSet<Global.Model.Cidade> Cidade { get; set; }
        public DbSet<Global.Model.Estado> Estado { get; set; }
        public DbSet<Global.Model.TipoTelefoneGlobal> TipoTelefoneGlobal { get; set; }
        public DbSet<Global.Model.TipoFuncaoGlobal> TipoFuncao { get; set; }
        public DbSet<Global.Model.CargoGlobal> CargoGlobal { get; set; }
        public DbSet<Global.Model.CategoriaCargoGlobal> TipoFuncaoGlobal { get; set; }
        public DbSet<Global.Model.Funcao> FuncaoGlobal { get; set; }
        public DbSet<Global.Model.FuncaoSinonimo> FuncaoSinonimoGlobal { get; set; }
        public DbSet<Global.Model.RamoAtividadeGlobal> RamoAtividade { get; set; }
        public DbSet<Global.Model.Sexo> Sexo { get; set; }
        public DbSet<Global.Model.GrauEscolaridadeGlobal> GrauEscolaridade { get; set; }
        public DbSet<Global.Model.EscolaridadeGlobal> Escolaridade { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            #region Pessoa Jurídica
            modelBuilder.Configurations.Add(new Configuration.UsuarioPessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.CNAEConfiguration());
            modelBuilder.Configurations.Add(new Configuration.NaturezaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioConfiguration());
            modelBuilder.Configurations.Add(new Configuration.PessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.PerfilConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TelefoneConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EnderecoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EmailConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioAdicionalConfiguration());
            modelBuilder.Configurations.Add(new Configuration.ParametroConfiguration());

            modelBuilder.Configurations.Add(new Global.Data.Configuration.BairroConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CidadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.EstadoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.TipoTelefoneConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.TipoFuncaoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CategoriaCargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.CargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.FuncaoSinonimoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.FuncaoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.RamoAtividadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.SexoConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.GrauEscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new Global.Data.Configuration.EscolaridadeConfiguration());
            #endregion

            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

    }
}
