using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class TelefoneConfiguration : EntityTypeConfiguration<Telefone>
    {
        public TelefoneConfiguration()
        {
            ToTable("Telefone", "pessoajuridica");
            Property(p => p.Id).HasColumnName("IdTelefone");
            Property(n => n.Numero).HasPrecision(10, 0);
            Property(n => n.Ramal).HasPrecision(5, 0);
            Map<TelefoneFixo>(n => n.Requires("IdTipoTelefone").HasValue(1).IsOptional());
            Map<TelefoneCelular>(n => n.Requires("IdTipoTelefone").HasValue(2).IsOptional());
            Map<TelefoneComercial>(n => n.Requires("IdTipoTelefone").HasValue(3).IsOptional());
            HasOptional(n => n.PessoaJuridica).WithMany().Map(n => n.MapKey("IdPessoaJuridica"));
            HasOptional(n => n.Usuario).WithMany().Map(n => n.MapKey("IdUsuario"));
            HasOptional(n => n.UsuarioPessoaJuridica).WithMany().Map(n => n.MapKey("IdUsuarioUsuarioPessoaJuridica", "IdPessoaJuridicaUsuarioPessoaJuridica"));
        }
    }
}