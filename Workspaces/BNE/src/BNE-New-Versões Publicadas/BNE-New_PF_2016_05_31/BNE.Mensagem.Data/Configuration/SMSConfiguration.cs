using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class SMSConfiguration : EntityTypeConfiguration<Model.SMS>
    {

        public SMSConfiguration()
        {
            ToTable("SMS", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdSMS");
            this.Property(n => n.Numero).HasPrecision(10, 0);
            this.HasRequired(n => n.Status).WithMany().Map(n => n.MapKey("IdStatus"));
            this.HasRequired(n => n.TemplateSMS).WithMany().Map(n => n.MapKey("IdTemplateSMS", "VersaoTemplateSMS"));
            this.HasOptional(n => n.UsuarioDestinatario).WithMany().Map(n => n.MapKey("IdUsuarioDestinatario"));
            this.HasOptional(n => n.UsuarioRemetente).WithMany().Map(n => n.MapKey("IdUsuarioRemetente"));
        }

    }
}
