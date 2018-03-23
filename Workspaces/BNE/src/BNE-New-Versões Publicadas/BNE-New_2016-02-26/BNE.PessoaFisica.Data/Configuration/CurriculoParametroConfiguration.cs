using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoParametroConfiguration : EntityTypeConfiguration<CurriculoParametro>
    {
        public CurriculoParametroConfiguration()
        {
            ToTable("CurriculoParametro", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCurriculoParametro");
            this.HasKey(p => p.Id);
            this.Property(p => p.Valor).IsMaxLength();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();

            this.HasRequired(p=>p.Curriculo).WithMany().Map(p=>p.MapKey("IdCurriculo"));
            this.HasRequired(p => p.Parametro).WithMany().Map(p => p.MapKey("IdParametro"));
        }
    }
}