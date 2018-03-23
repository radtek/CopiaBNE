using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CidadePretendidaConfiguration : EntityTypeConfiguration<CidadePretendida>
    {
        public CidadePretendidaConfiguration()
        {
            ToTable("CidadePretendida", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCidadePretendida");
            this.HasKey(p => p.Id);

            this.Property(p => p.DataCadastro).IsRequired();
            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            this.HasRequired(p => p.CidadeGlobal).WithMany().Map(p => p.MapKey("IdCidadeGlobal"));
        }
    }
}