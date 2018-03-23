using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CidadePretendidaConfiguration : EntityTypeConfiguration<CidadePretendida>
    {
        public CidadePretendidaConfiguration()
        {
            ToTable("CidadePretendida", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCidadePretendida");
            HasKey(p => p.Id);

            Property(p => p.DataCadastro).IsRequired();
            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            HasRequired(p => p.CidadeGlobal).WithMany().Map(p => p.MapKey("IdCidadeGlobal"));
        }
    }
}