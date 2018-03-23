using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class PreCurriculoConfiguration : EntityTypeConfiguration<PreCurriculo>
    {
        public PreCurriculoConfiguration()
        {
            ToTable("PreCurriculo", "pessoafisica");
            this.Property(n => n.Id).HasColumnName("IdPreCurriculo");
            this.HasKey(n => n.Id);
            this.Property(n => n.Nome).HasMaxLength(100).IsOptional();
            this.Property(n => n.Email).HasMaxLength(100).IsOptional();
            this.Property(n => n.DDDCelular).HasMaxLength(2).IsOptional();
            this.Property(n => n.Celular).HasMaxLength(10).IsOptional();
            this.Property(n => n.IdFuncao).IsOptional();
            this.Property(n => n.IdCidade).IsOptional();
            this.Property(n => n.TempoExperiencia).IsOptional();
            this.Property(n => n.PretensaoSalarial).HasPrecision(10, 2).IsOptional();
            this.Property(n => n.IdVaga).IsOptional();
            this.Property(n => n.IdCurriculo).IsOptional();
            this.Property(n => n.DataCadastro).IsRequired();
            this.Property(n => n.DescricaoFuncao).HasMaxLength(50).IsOptional();
        }
    }
}