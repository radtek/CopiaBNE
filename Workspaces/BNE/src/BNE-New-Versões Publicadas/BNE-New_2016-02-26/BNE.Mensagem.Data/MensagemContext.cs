using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BNE.Mensagem.Data
{
    public class MensagemContext : DbContext
    {
        public MensagemContext()
            : base("name=Mensagem")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Model.Anexo> Anexo { get; set; }
        public DbSet<Model.Email> Email { get; set; }
        public DbSet<Model.SMS> SMS { get; set; }
        public DbSet<Model.Sistema> Sistema { get; set; }
        public DbSet<Model.Status> Status { get; set; }
        public DbSet<Model.TemplateSMS> TemplateSMS { get; set; }
        public DbSet<Model.TemplateEmail> TemplateEmail { get; set; }
        public DbSet<Model.Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new Configuration.AnexoConfiguration());
            modelBuilder.Configurations.Add(new Configuration.EmailConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SMSConfiguration());
            modelBuilder.Configurations.Add(new Configuration.SistemaConfiguration());
            modelBuilder.Configurations.Add(new Configuration.StatusConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TemplateSMSConfiguration());
            modelBuilder.Configurations.Add(new Configuration.TemplateEmailConfiguration());
            modelBuilder.Configurations.Add(new Configuration.UsuarioConfiguration());

            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

    }
}
