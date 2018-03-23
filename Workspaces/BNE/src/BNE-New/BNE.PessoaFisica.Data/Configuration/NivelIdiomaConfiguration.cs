using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class NivelIdiomaConfiguration : EntityTypeConfiguration<NivelIdioma>
    {
        public NivelIdiomaConfiguration()
        {
            ToTable("NivelIdioma", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdNivelIdioma");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}