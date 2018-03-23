using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class TemplateEmailConfiguration : EntityTypeConfiguration<Model.TemplateEmail>
    {

        public TemplateEmailConfiguration()
        {
            ToTable("TemplateEmail", "mensagem");
            this.HasKey(n => new { n.Id, n.Versao });
            this.Property(n => n.Id).HasColumnName("IdTemplateEmail");
            this.Property(n => n.Nome).HasMaxLength(50).IsRequired();
            this.Property(n => n.Assunto).HasMaxLength(200).IsOptional();
            this.Property(n => n.Conteudo).IsRequired();
            this.Property(n => n.Versao).IsRequired();
            this.HasRequired(n => n.Sistema).WithMany().Map(n => n.MapKey("IdSistema"));
            this.HasOptional(n => n.TemplateSistema).WithMany().Map(n => n.MapKey("IdTemplateSistema", "VersaoTemplateSistema"));
        }

    }
}
