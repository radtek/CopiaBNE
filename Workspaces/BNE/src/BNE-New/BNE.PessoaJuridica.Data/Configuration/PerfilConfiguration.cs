using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class PerfilConfiguration : EntityTypeConfiguration<Perfil>
    {
        public PerfilConfiguration()
        {
            ToTable("Perfil", "pessoajuridica");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdPerfil").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(n => n.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}