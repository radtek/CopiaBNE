using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class SituacaoFormacaoConfiguration : EntityTypeConfiguration<SituacaoFormacao>
    {
        public SituacaoFormacaoConfiguration()
        {
            ToTable("SituacaoFormacao", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdSituacaoFormacao");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}