using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using BNE.Global.Data.Configuration;
using BNE.Global.Model;
using BNE.PessoaJuridica.Data.Configuration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data
{
    public class PessoaJuridicaContext : DbContext
    {
        public PessoaJuridicaContext()
            : base("name=PessoaJuridica")
        {
            var instance = SqlProviderServices.Instance;
        }

        public DbSet<Domain.Model.PessoaJuridica> PessoaJuridica { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<Usuario> UsuarioPessoaJuridica { get; set; }
        public DbSet<UsuarioPessoaJuridica> UsuarioPessoaJuridicaPerfil { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<NaturezaJuridica> NaturezaJuridica { get; set; }
        public DbSet<CNAE> CNAE { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<UsuarioAdicional> UsuarioAdicional { get; set; }
        public DbSet<Parametro> Parametro { get; set; }

        public DbSet<Bairro> Bairro { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<TipoTelefoneGlobal> TipoTelefoneGlobal { get; set; }
        public DbSet<TipoFuncaoGlobal> TipoFuncao { get; set; }
        public DbSet<CargoGlobal> CargoGlobal { get; set; }
        public DbSet<CategoriaCargoGlobal> TipoFuncaoGlobal { get; set; }
        public DbSet<Funcao> FuncaoGlobal { get; set; }
        public DbSet<FuncaoSinonimo> FuncaoSinonimoGlobal { get; set; }
        public DbSet<RamoAtividadeGlobal> RamoAtividade { get; set; }
        public DbSet<Sexo> Sexo { get; set; }
        public DbSet<GrauEscolaridadeGlobal> GrauEscolaridade { get; set; }
        public DbSet<EscolaridadeGlobal> Escolaridade { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            #region Pessoa Jurídica

            modelBuilder.Configurations.Add(new UsuarioPessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new CNAEConfiguration());
            modelBuilder.Configurations.Add(new NaturezaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new PessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new PerfilConfiguration());
            modelBuilder.Configurations.Add(new TelefoneConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());
            modelBuilder.Configurations.Add(new EmailConfiguration());
            modelBuilder.Configurations.Add(new UsuarioAdicionalConfiguration());
            modelBuilder.Configurations.Add(new ParametroConfiguration());

            modelBuilder.Configurations.Add(new BairroConfiguration());
            modelBuilder.Configurations.Add(new CidadeConfiguration());
            modelBuilder.Configurations.Add(new EstadoConfiguration());
            modelBuilder.Configurations.Add(new TipoTelefoneConfiguration());
            modelBuilder.Configurations.Add(new TipoFuncaoGlobalConfiguration());
            modelBuilder.Configurations.Add(new CategoriaCargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new CargoGlobalConfiguration());
            modelBuilder.Configurations.Add(new FuncaoSinonimoConfiguration());
            modelBuilder.Configurations.Add(new FuncaoConfiguration());
            modelBuilder.Configurations.Add(new RamoAtividadeConfiguration());
            modelBuilder.Configurations.Add(new SexoConfiguration());
            modelBuilder.Configurations.Add(new GrauEscolaridadeConfiguration());
            modelBuilder.Configurations.Add(new EscolaridadeConfiguration());

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