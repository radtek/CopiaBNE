using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TipoCurriculoConfiguration : EntityTypeConfiguration<TipoCurriculo>
    {
        public TipoCurriculoConfiguration()
        {
            ToTable("TipoCurriculo", "pessoafisica");
            Property(x => x.Id).HasColumnName("IdTipoCurriculo");
            HasKey(x => x.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}