using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class EmailConfiguration : EntityTypeConfiguration<Email>
    {
        public EmailConfiguration()
        {
            ToTable("Email", "pessoajuridica");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdEmail");
            Property(p => p.Endereco).HasMaxLength(100).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            HasOptional(n => n.PessoaJuridica).WithMany(n => n.Email).Map(n => n.MapKey("IdPessoaJuridica"));
            HasOptional(n => n.Usuario).WithMany().Map(n => n.MapKey("IdUsuario"));
            HasOptional(n => n.UsuarioPessoaJuridica).WithMany(n => n.Email).Map(n => n.MapKey("IdUsuarioUsuarioPessoaJuridica", "IdPessoaJuridicaUsuarioPessoaJuridica"));
        }
    }
}