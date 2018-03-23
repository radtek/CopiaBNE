using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class NivelIdiomaConfiguration : EntityTypeConfiguration<NivelIdioma>
    {
        public NivelIdiomaConfiguration()
        {
            ToTable("NivelIdioma", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdNivelIdioma");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}