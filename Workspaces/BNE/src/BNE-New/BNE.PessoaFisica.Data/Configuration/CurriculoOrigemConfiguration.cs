using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoOrigemConfiguration : EntityTypeConfiguration<CurriculoOrigem>
    {
        public CurriculoOrigemConfiguration()
        {
            ToTable("CurriculoOrigem", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCurriculoOrigem");
            HasKey(p => p.Id);
            Property(p => p.DataCadastro).IsRequired();

            HasRequired(p => p.OrigemGlobal).WithMany().Map(p => p.MapKey("IdOrigem"));
            HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
        }
    }
}