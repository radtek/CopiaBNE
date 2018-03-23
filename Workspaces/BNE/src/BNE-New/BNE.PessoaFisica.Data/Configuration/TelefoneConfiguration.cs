using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TelefoneConfiguration : EntityTypeConfiguration<Telefone>
    {
        public TelefoneConfiguration()
        {
            ToTable("Telefone", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdTelefone");
            Property(p => p.Numero).HasPrecision(10, 0);
            Property(p => p.DDD);
            Property(p => p.Ramal).HasPrecision(5, 0);
            Property(p => p.FalarCom).HasMaxLength(50).IsOptional();

            HasOptional(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));

            Map<TelefoneResidencial>(n => n.Requires("IdTipoTelefone").HasValue(1));
            Map<TelefoneCelular>(n => n.Requires("IdTipoTelefone").HasValue(2));
            Map<TelefoneRecado>(n => n.Requires("IdTipoTelefone").HasValue(3));
        }
    }
}