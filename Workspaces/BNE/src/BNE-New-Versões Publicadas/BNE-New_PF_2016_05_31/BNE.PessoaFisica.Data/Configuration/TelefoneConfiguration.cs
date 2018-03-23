using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TelefoneConfiguration: EntityTypeConfiguration<Telefone>
    {
        public TelefoneConfiguration()
        {
            ToTable("Telefone", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdTelefone");
            this.Property(p=>p.Numero).HasPrecision(10,0);
            this.Property(p => p.DDD);
            this.Property(p => p.Ramal).HasPrecision(5,0);
            this.Property(p=>p.FalarCom).HasMaxLength(50).IsOptional();

            this.HasOptional(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));

            this.Map<TelefoneResidencial>(n => n.Requires("IdTipoTelefone").HasValue(1));
            this.Map<TelefoneCelular>(n => n.Requires("IdTipoTelefone").HasValue(2));
            this.Map<TelefoneRecado>(n => n.Requires("IdTipoTelefone").HasValue(3));
        }
    }
}