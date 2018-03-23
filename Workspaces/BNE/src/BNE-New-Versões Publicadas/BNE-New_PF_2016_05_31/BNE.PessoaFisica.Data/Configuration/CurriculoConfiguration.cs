using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoConfiguration: EntityTypeConfiguration<Curriculo>
    {
        public CurriculoConfiguration()
        {
            ToTable("Curriculo", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCurriculo");
            this.HasKey(p => p.Id);

            this.Property(p => p.PretensaoSalarial).HasPrecision(10,2).IsRequired();
            this.Property(p=>p.DataCadastro).IsRequired();
            this.Property(p => p.DataAtualizacao).IsOptional();
            this.Property(p => p.DataModificacao).IsOptional();
            this.Property(p => p.Observacao).HasMaxLength(2000).IsOptional();
            this.Property(p => p.Conhecimento).HasMaxLength(2000).IsOptional();
            this.Property(p => p.FlgVIP).IsRequired();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.FlgDisponivelViagem).IsRequired();

            this.HasRequired(p => p.TipoCurriculo).WithMany().Map(p => p.MapKey("IdTipoCurriculo"));
            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            this.HasRequired(p => p.SituacaoCurriculo).WithMany().Map(p => p.MapKey("IdSituacaoCurriculo"));
        }
    }
}