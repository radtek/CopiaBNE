using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TipoVeiculoConfiguration : EntityTypeConfiguration<TipoVeiculo>
    {
        public TipoVeiculoConfiguration()
        {
            ToTable("TipoVeiculo", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdTipoVeiculo");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}