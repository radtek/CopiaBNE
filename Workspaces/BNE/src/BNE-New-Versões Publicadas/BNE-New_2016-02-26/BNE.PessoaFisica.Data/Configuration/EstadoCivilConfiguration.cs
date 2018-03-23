using BNE.PessoaFisica.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class EstadoCivilConfiguration : EntityTypeConfiguration<EstadoCivil>
    {
        public EstadoCivilConfiguration()
        {
            ToTable("EstadoCivil", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdEstadoCivil");
            this.Property(n => n.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}
