using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioPessoaJuridicaConfiguration : EntityTypeConfiguration<UsuarioPessoaJuridica>
    {
        public UsuarioPessoaJuridicaConfiguration()
        {
            ToTable("UsuarioPessoaJuridica", "pessoajuridica");
            Property(n => n.Funcao).HasMaxLength(100);
            HasRequired(n => n.Perfil).WithMany().Map(n => n.MapKey("IdPerfil"));
            HasRequired(n => n.Usuario).WithMany().HasForeignKey(n => n.IdUsuario);
            HasRequired(n => n.PessoaJuridica).WithMany(n => n.UsuarioPessoaJuridica).HasForeignKey(n => n.IdPessoaJuridica);
            Property(n => n.IP).HasMaxLength(15).IsRequired();
            HasKey(n => new {n.IdUsuario, n.IdPessoaJuridica});
            HasOptional(n => n.FuncaoSinonimo).WithMany().Map(n => n.MapKey("IdFuncaoSinonimo")).WillCascadeOnDelete(false);
        }
    }
}