using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FormacaoConfiguration : EntityTypeConfiguration<Formacao>
    {
        public FormacaoConfiguration()
        {
            ToTable("Formacao", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdFormacao");
            HasKey(p => p.Id);
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAlteracao).IsOptional();
            Property(p => p.AnoConclusao).IsOptional();
            Property(p => p.CargaHoraria).IsOptional();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.NomeCurso).HasMaxLength(200).IsOptional();
            Property(p => p.NomeInstituicao).HasMaxLength(200).IsOptional();

            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            HasRequired(p => p.EscolaridadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaridadeGlobal"));
            HasOptional(p => p.Curso).WithMany().Map(p => p.MapKey("IdCurso"));
            HasOptional(p => p.Cidade).WithMany().Map(p => p.MapKey("IdCidade"));
            HasOptional(p => p.InstituicaoEnsino).WithMany().Map(p => p.MapKey("IdInstituicaoEnsino"));
        }
    }
}