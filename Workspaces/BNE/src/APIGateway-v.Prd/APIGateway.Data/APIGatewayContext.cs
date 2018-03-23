using log4net;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace APIGateway.Data
{
    public class APIGatewayContext : DbContext
    {
        private static readonly ILog _logger = LogManager.GetLogger("GatewayAPI");

        public APIGatewayContext()
            : base("name=APIGateway")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Model.Api> Api { get; set; }
        public DbSet<Model.Authentication> Authentication { get; set; }
        public DbSet<Model.OAuthConfig> OAuthConfig { get; set; }
        public DbSet<Model.SwaggerConfig> SwaggerConfig { get; set; }
        public DbSet<Model.Authorization> Authorization { get; set; }
        public DbSet<Model.Endpoint> Endpoint { get; set; }
        public DbSet<Model.Requisicao> Requisicao { get; set; }
        public DbSet<Model.SistemaCliente> SistemaCliente { get; set; }
        public DbSet<Model.Usuario> Usuario { get; set; }
        public DbSet<Model.UsuarioPessoaFisica> UsuarioPessoaFisica { get; set; }
        public DbSet<Model.UsuarioSistemaCliente> UsuarioSistemaCliente { get; set; }
        public DbSet<Model.UsuarioPessoaJuridica> UsuarioPessoaJuridica { get; set; }
        public DbSet<Model.Header> Header { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            #region Configurations
            modelBuilder.Configurations.Add(new Configuration.ApiConfiguration());
            modelBuilder.Configurations.Add(new Configuration.AuthenticationConfiguration());
            modelBuilder.Configurations.Add(new Configuration.OAuthConfigConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SwaggerConfigConfiguration());
            modelBuilder.Configurations.Add(new Configuration.AuthorizationConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EndpointConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SistemaClienteConfiguration());
            modelBuilder.Configurations.Add(new Configuration.RequisicaoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioPessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioSistemaClienteConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioPessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.HeaderConfiguration());
            #endregion

            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                _logger.Error(ex);
                throw (ex);
            }
        }

    }
}
