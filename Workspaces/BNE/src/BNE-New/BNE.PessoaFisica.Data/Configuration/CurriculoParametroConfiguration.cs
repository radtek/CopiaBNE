using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoParametroConfiguration : EntityTypeConfiguration<CurriculoParametro>
    {
        public CurriculoParametroConfiguration()
        {
            ToTable("CurriculoParametro", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCurriculoParametro");
            HasKey(p => p.Id);
            Property(p => p.Valor).IsMaxLength();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.DataCadastro).IsRequired();

            HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
            HasRequired(p => p.Parametro).WithMany().Map(p => p.MapKey("IdParametro"));
        }
    }
}