using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioPessoaJuridicaConfiguration : EntityTypeConfiguration<Model.UsuarioPessoaJuridica>
    {

        public UsuarioPessoaJuridicaConfiguration()
        {
            ToTable("UsuarioPessoaJuridica", "pessoajuridica");
            this.Property(n => n.Funcao).HasMaxLength(100);
            this.HasRequired(n => n.Perfil).WithMany().Map(n => n.MapKey("IdPerfil"));
            this.HasRequired(n => n.Usuario).WithMany().HasForeignKey(n => n.IdUsuario);
            this.HasRequired(n => n.PessoaJuridica).WithMany(n => n.UsuarioPessoaJuridica).HasForeignKey(n => n.IdPessoaJuridica);
            this.Property(n => n.IP).HasMaxLength(15).IsRequired();
            this.HasKey(n => new { n.IdUsuario, n.IdPessoaJuridica });
            this.HasOptional(n => n.FuncaoSinonimo).WithMany().Map(n => n.MapKey("IdFuncaoSinonimo")).WillCascadeOnDelete(false);
        }

    }
}
