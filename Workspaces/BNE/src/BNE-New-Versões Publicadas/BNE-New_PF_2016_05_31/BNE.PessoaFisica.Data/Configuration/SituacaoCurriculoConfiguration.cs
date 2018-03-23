using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class SituacaoCurriculoConfiguration : EntityTypeConfiguration<Model.SituacaoCurriculo>
    {
        public SituacaoCurriculoConfiguration()
        {
            ToTable("SituacaoCurriculo", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdSituacaoCurriculo");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}		