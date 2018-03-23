using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioAdicionalConfiguration : EntityTypeConfiguration<Model.UsuarioAdicional>
    {

        public UsuarioAdicionalConfiguration()
        {
            ToTable("UsuarioAdicional", "pessoajuridica");
            this.HasKey(n => n.Id);
            this.Property(n => n.Nome).HasMaxLength(200).IsRequired();
            this.Property(n => n.Email).HasMaxLength(200).IsRequired();
            this.HasRequired(n => n.PessoaJuridica).WithMany().Map(n => n.MapKey("IdPessoaJuridica"));
        }

    }
}
