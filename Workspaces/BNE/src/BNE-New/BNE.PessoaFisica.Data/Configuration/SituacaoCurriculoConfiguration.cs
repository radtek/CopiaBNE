using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class SituacaoCurriculoConfiguration : EntityTypeConfiguration<SituacaoCurriculo>
    {
        public SituacaoCurriculoConfiguration()
        {
            ToTable("SituacaoCurriculo", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdSituacaoCurriculo");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}