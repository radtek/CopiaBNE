using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class TelefoneConfiguration : EntityTypeConfiguration<Telefone>
    {

        public TelefoneConfiguration()
        {
            ToTable("Telefone", "pessoajuridica");
            this.Property(p => p.Id).HasColumnName("IdTelefone");
            this.Property(n => n.Numero).HasPrecision(10, 0);
            this.Property(n => n.Ramal).HasPrecision(5, 0);
            this.Map<TelefoneFixo>(n => n.Requires("IdTipoTelefone").HasValue(1).IsOptional());
            this.Map<TelefoneCelular>(n => n.Requires("IdTipoTelefone").HasValue(2).IsOptional());
            this.Map<TelefoneComercial>(n => n.Requires("IdTipoTelefone").HasValue(3).IsOptional());
            this.HasOptional(n => n.PessoaJuridica).WithMany().Map(n => n.MapKey("IdPessoaJuridica"));
            this.HasOptional(n => n.Usuario).WithMany().Map(n => n.MapKey("IdUsuario"));
            this.HasOptional(n => n.UsuarioPessoaJuridica).WithMany().Map(n => n.MapKey("IdUsuarioUsuarioPessoaJuridica", "IdPessoaJuridicaUsuarioPessoaJuridica"));
        }

    }
}
