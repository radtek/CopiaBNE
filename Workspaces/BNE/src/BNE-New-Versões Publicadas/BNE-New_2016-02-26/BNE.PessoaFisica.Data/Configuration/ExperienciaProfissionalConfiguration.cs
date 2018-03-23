using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class ExperienciaProfissionalConfiguration : EntityTypeConfiguration<ExperienciaProfissional>
    {
        public ExperienciaProfissionalConfiguration()
        {
            ToTable("ExperienciaProfissional", "pessoafisica");
            this.Property(p => p.id).HasColumnName("IdExperienciaProfissional");
            this.HasKey(p => p.id);
            this.Property(p => p.NomeEmpresa).HasMaxLength(100).IsRequired();
            this.Property(p => p.FuncaoExercida).HasMaxLength(100).IsOptional();
            this.Property(p => p.DataEntrada).HasColumnType("Date").IsRequired();
            this.Property(p => p.DataSaida).HasColumnType("Date").IsOptional();
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.UltimoSalario).HasPrecision(10, 2).IsOptional();
            this.Property(p => p.AtividadesExercidas).IsMaxLength();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.FlgImportado).IsOptional();

            this.HasOptional(p => p.RamoAtividadeGlobal).WithMany().Map(p => p.MapKey("IdRamoAtividadeGlobal"));
            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));

        }
    }
}