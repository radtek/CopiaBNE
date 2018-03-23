using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class ExperienciaProfissionalConfiguration : EntityTypeConfiguration<ExperienciaProfissional>
    {
        public ExperienciaProfissionalConfiguration()
        {
            ToTable("ExperienciaProfissional", "pessoafisica");
            Property(p => p.id).HasColumnName("IdExperienciaProfissional");
            HasKey(p => p.id);
            Property(p => p.NomeEmpresa).HasMaxLength(100).IsRequired();
            Property(p => p.FuncaoExercida).HasMaxLength(100).IsOptional();
            Property(p => p.DataEntrada).HasColumnType("Date").IsRequired();
            Property(p => p.DataSaida).HasColumnType("Date").IsOptional();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.UltimoSalario).HasPrecision(10, 2).IsOptional();
            Property(p => p.AtividadesExercidas).IsMaxLength();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.FlgImportado).IsOptional();

            HasOptional(p => p.RamoAtividadeGlobal).WithMany().Map(p => p.MapKey("IdRamoAtividadeGlobal"));
            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
        }
    }
}