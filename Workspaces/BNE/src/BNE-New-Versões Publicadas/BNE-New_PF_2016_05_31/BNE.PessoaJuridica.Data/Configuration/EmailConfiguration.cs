using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class EmailConfiguration : EntityTypeConfiguration<Model.Email>
    {
        public EmailConfiguration()
        {
            ToTable("Email", "pessoajuridica");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnName("IdEmail");
            this.Property(p => p.Endereco).HasMaxLength(100).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
            this.HasOptional(n => n.PessoaJuridica).WithMany(n => n.Email).Map(n => n.MapKey("IdPessoaJuridica"));
            this.HasOptional(n => n.Usuario).WithMany().Map(n => n.MapKey("IdUsuario"));
            this.HasOptional(n => n.UsuarioPessoaJuridica).WithMany(n => n.Email).Map(n => n.MapKey("IdUsuarioUsuarioPessoaJuridica", "IdPessoaJuridicaUsuarioPessoaJuridica"));
        }
    }
}