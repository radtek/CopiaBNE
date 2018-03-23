using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class VeiculoConfiguration : EntityTypeConfiguration<Veiculo>
    {
        public VeiculoConfiguration()
        {
            ToTable("Veiculo","pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdVeiculo");
            this.HasKey(p => p.Id);
            this.Property(p => p.Modelo).HasMaxLength(50).IsOptional();
            this.Property(p => p.Ano).IsRequired();

            this.HasRequired(p => p.TipoVeiculo).WithMany().Map(p => p.MapKey("IdTipoVeiculo"));
            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
        }
    }
}