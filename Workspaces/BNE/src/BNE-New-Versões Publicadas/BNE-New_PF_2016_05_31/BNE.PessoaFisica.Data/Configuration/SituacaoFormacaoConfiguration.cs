using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class SituacaoFormacaoConfiguration : EntityTypeConfiguration<SituacaoFormacao>
    {
        public SituacaoFormacaoConfiguration()
        {
            ToTable("SituacaoFormacao", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdSituacaoFormacao");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}