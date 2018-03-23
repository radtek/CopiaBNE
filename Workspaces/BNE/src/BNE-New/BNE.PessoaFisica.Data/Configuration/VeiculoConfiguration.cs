using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class VeiculoConfiguration : EntityTypeConfiguration<Veiculo>
    {
        public VeiculoConfiguration()
        {
            ToTable("Veiculo", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdVeiculo");
            HasKey(p => p.Id);
            Property(p => p.Modelo).HasMaxLength(50).IsOptional();
            Property(p => p.Ano).IsRequired();

            HasRequired(p => p.TipoVeiculo).WithMany().Map(p => p.MapKey("IdTipoVeiculo"));
            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
        }
    }
}