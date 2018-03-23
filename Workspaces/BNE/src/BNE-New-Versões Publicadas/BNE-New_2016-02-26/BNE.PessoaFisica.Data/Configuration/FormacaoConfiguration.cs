using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FormacaoConfiguration : EntityTypeConfiguration<Formacao>
    {
        public FormacaoConfiguration()
        {
            ToTable("Formacao", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdFormacao");
            this.HasKey(p => p.Id);
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.DataAlteracao).IsOptional();
            this.Property(p => p.AnoConclusao).IsOptional();
            this.Property(p => p.CargaHoraria).IsOptional();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.NomeCurso).HasMaxLength(200).IsOptional();
            this.Property(p => p.NomeInstituicao).HasMaxLength(200).IsOptional();

            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            this.HasRequired(p => p.EscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaridadeGlobal"));
            this.HasOptional(p => p.Curso).WithMany().Map(p => p.MapKey("IdCurso"));
            this.HasOptional(p => p.Cidade).WithMany().Map(p => p.MapKey("IdCidade"));
            this.HasOptional(p => p.InstituicaoEnsino).WithMany().Map(p => p.MapKey("IdInstituicaoEnsino"));
        }
    }
}