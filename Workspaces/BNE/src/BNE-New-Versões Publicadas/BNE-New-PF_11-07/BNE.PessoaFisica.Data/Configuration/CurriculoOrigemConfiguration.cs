using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoOrigemConfiguration : EntityTypeConfiguration<Model.CurriculoOrigem>
    {
        public CurriculoOrigemConfiguration()
        {
            ToTable("CurriculoOrigem", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCurriculoOrigem");
            this.HasKey(p => p.Id);
            this.Property(p => p.DataCadastro).IsRequired();

            this.HasRequired(p => p.OrigemGlobal).WithMany().Map(p=>p.MapKey("IdOrigem"));
            this.HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
        }
    }
}