using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoAnexoConfiguration : EntityTypeConfiguration<CurriculoAnexo>
    {
        public CurriculoAnexoConfiguration()
        {
            ToTable("CurriculoAnexo", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCurriculoAnexo");
            this.HasKey(p => p.Id);
            this.Property(p => p.DataCadastro).HasColumnType("Date").IsRequired();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.UrlArquivo).IsRequired();

            this.HasRequired(p => p.Curriculo).WithMany().Map(p=>p.MapKey("IdCurriculo"));
        }
    }
}