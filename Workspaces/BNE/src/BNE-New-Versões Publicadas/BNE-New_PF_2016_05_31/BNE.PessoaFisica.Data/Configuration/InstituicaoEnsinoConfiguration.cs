using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class InstituicaoEnsinoConfiguration : EntityTypeConfiguration<InstituicaoEnsino>
    {
        public InstituicaoEnsinoConfiguration()
        {
            ToTable("InstituicaoEnsino", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdInstituicaoEnsino");
            this.HasKey(p => p.Id);
            this.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            this.Property(p => p.Sigla).HasMaxLength(20).IsRequired();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.FlgMEC).IsRequired();
        }
    }
}