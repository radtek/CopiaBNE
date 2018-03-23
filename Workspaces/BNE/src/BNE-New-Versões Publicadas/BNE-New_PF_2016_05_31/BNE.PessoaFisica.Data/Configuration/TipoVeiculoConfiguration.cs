using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TipoVeiculoConfiguration: EntityTypeConfiguration<TipoVeiculo>
    {
        public TipoVeiculoConfiguration()
        {
            ToTable("TipoVeiculo", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdTipoVeiculo");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}