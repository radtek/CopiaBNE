using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioAdicionalConfiguration : EntityTypeConfiguration<UsuarioAdicional>
    {
        public UsuarioAdicionalConfiguration()
        {
            ToTable("UsuarioAdicional", "pessoajuridica");
            HasKey(n => n.Id);
            Property(n => n.Nome).HasMaxLength(200).IsRequired();
            Property(n => n.Email).HasMaxLength(200).IsRequired();
            HasRequired(n => n.PessoaJuridica).WithMany().Map(n => n.MapKey("IdPessoaJuridica"));
        }
    }
}