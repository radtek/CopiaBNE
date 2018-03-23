using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoConfiguration : EntityTypeConfiguration<Curriculo>
    {
        public CurriculoConfiguration()
        {
            ToTable("Curriculo", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCurriculo");
            HasKey(p => p.Id);

            Property(p => p.PretensaoSalarial).HasPrecision(10, 2).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAtualizacao).IsOptional();
            Property(p => p.DataModificacao).IsOptional();
            Property(p => p.Observacao).HasMaxLength(2000).IsOptional();
            Property(p => p.Conhecimento).HasMaxLength(2000).IsOptional();
            Property(p => p.FlgVIP).IsRequired();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.FlgDisponivelViagem).IsRequired();

            HasRequired(p => p.TipoCurriculo).WithMany().Map(p => p.MapKey("IdTipoCurriculo"));
            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            HasRequired(p => p.SituacaoCurriculo).WithMany().Map(p => p.MapKey("IdSituacaoCurriculo"));
        }
    }
}