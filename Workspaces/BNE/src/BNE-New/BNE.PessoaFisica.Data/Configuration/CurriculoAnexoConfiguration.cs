using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoAnexoConfiguration : EntityTypeConfiguration<CurriculoAnexo>
    {
        public CurriculoAnexoConfiguration()
        {
            ToTable("CurriculoAnexo", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCurriculoAnexo");
            HasKey(p => p.Id);
            Property(p => p.DataCadastro).HasColumnType("Date").IsRequired();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.UrlArquivo).IsRequired();

            HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
        }
    }
}