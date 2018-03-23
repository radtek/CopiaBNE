using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class TemplateSMSConfiguration : EntityTypeConfiguration<Model.TemplateSMS>
    {

        public TemplateSMSConfiguration()
        {
            ToTable("TemplateSMS", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdTemplateSMS");
            this.Property(n => n.Nome).HasMaxLength(50).IsRequired();
            this.Property(n => n.Conteudo).IsRequired();
            this.Property(n => n.Versao).IsRequired();
            this.HasRequired(n => n.Sistema).WithMany().Map(n => n.MapKey("IdSistema"));
            this.HasKey(n => new { n.Id, n.Versao });
        }

    }
}
