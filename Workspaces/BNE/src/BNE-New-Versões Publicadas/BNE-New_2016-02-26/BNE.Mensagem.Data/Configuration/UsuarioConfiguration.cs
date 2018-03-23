using System.Data.Entity.ModelConfiguration;

namespace BNE.Mensagem.Data.Configuration
{
    class UsuarioConfiguration : EntityTypeConfiguration<Model.Usuario>
    {

        public UsuarioConfiguration()
        {
            ToTable("Usuario", "mensagem");
            this.Property(n => n.Id).HasColumnName("IdUsuario");
            this.HasRequired(n => n.Sistema).WithMany().Map(n => n.MapKey("IdSistema"));
        }

    }
}
