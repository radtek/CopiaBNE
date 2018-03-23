using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class IdiomaConfiguration : EntityTypeConfiguration<Idioma>
    {
        public IdiomaConfiguration()
        {
            ToTable("Idioma", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdIdioma");
            HasKey(p => p.Id);
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.Ativo).IsRequired();

            HasRequired(p => p.IdiomaGlobal).WithMany().Map(p => p.MapKey("IdIdiomaGlobal"));
            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            HasRequired(p => p.NivelIdioma).WithMany().Map(p => p.MapKey("IdNivelIdioma"));
        }
    }
}