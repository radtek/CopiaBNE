using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class EmailConfiguration : EntityTypeConfiguration<Model.Email>
    {

        public EmailConfiguration()
        {
            ToTable("Email", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdEmail");
            this.Property(n => n.EmailRemetente).HasMaxLength(100).IsRequired();
            this.Property(n => n.EmailDestinatario).HasMaxLength(100).IsRequired();
            this.HasRequired(n => n.Status).WithMany().Map(n => n.MapKey("IdStatus"));
            this.HasRequired(n => n.TemplateEmail).WithMany().Map(n => n.MapKey("IdTemplateEmail", "VersaoTemplateEmail"));
            this.HasOptional(n => n.Anexo).WithMany().Map(n => n.MapKey("IdAnexo"));
            this.HasOptional(n => n.UsuarioDestinatario).WithMany().Map(n => n.MapKey("IdUsuarioDestinatario"));
            this.HasOptional(n => n.UsuarioRemetente).WithMany().Map(n => n.MapKey("IdUsuarioRemetente"));
        }

    }
}
