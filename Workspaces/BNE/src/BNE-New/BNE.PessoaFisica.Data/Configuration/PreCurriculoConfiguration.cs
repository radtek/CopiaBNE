using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class PreCurriculoConfiguration : EntityTypeConfiguration<PreCurriculo>
    {
        public PreCurriculoConfiguration()
        {
            ToTable("PreCurriculo", "pessoafisica");
            Property(n => n.Id).HasColumnName("IdPreCurriculo");
            HasKey(n => n.Id);
            Property(n => n.Nome).HasMaxLength(100).IsOptional();
            Property(n => n.Email).HasMaxLength(100).IsOptional();
            Property(n => n.DDDCelular).HasMaxLength(2).IsOptional();
            Property(n => n.Celular).HasMaxLength(10).IsOptional();
            Property(n => n.IdFuncao).IsOptional();
            Property(n => n.IdCidade).IsOptional();
            Property(n => n.TempoExperiencia).IsOptional();
            Property(n => n.PretensaoSalarial).HasPrecision(10, 2).IsOptional();
            Property(n => n.IdVaga).IsOptional();
            Property(n => n.IdCurriculo).IsOptional();
            Property(n => n.DataCadastro).IsRequired();
            Property(n => n.DescricaoFuncao).HasMaxLength(50).IsOptional();

            HasOptional(p => p.Sexo).WithMany().Map(p => p.MapKey("SiglaSexo"));
        }
    }
}