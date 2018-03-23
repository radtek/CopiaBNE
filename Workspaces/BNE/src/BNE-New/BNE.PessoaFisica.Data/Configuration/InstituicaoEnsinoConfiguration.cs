using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class InstituicaoEnsinoConfiguration : EntityTypeConfiguration<InstituicaoEnsino>
    {
        public InstituicaoEnsinoConfiguration()
        {
            ToTable("InstituicaoEnsino", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdInstituicaoEnsino");
            HasKey(p => p.Id);
            Property(p => p.Nome).HasMaxLength(100).IsRequired();
            Property(p => p.Sigla).HasMaxLength(20).IsRequired();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.FlgMEC).IsRequired();
        }
    }
}