using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class PerfilConfiguration : EntityTypeConfiguration<Model.Perfil>
    {

        public PerfilConfiguration()
        {
            ToTable("Perfil", "pessoajuridica");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdPerfil").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(n => n.Descricao).HasMaxLength(30).IsRequired();
        }

    }
}
