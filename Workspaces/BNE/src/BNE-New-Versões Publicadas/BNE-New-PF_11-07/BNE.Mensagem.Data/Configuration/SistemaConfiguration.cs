using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class SistemaConfiguration : EntityTypeConfiguration<Model.Sistema>
    {

        public SistemaConfiguration()
        {
            ToTable("Sistema", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdSistema");
            this.Property(n => n.Nome).HasMaxLength(20).IsRequired();
            this.Property(n => n.UrlSite).HasMaxLength(100);
            this.Property(n => n.UrlImagens).HasMaxLength(100);
        }

    }
}
