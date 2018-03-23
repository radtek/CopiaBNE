using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            ToTable("Usuario", "pessoajuridica");
            Property(n => n.Nome).HasMaxLength(200).IsRequired();
            Property(n => n.DataNascimento).HasColumnType("DATE").IsRequired();
            Property(n => n.CPF).HasPrecision(11, 0).IsRequired();
            Property(n => n.IP).HasMaxLength(15).IsRequired();
            Property(n => n.DataAlteracao).IsRequired();
            Property(n => n.DataCadastro).IsRequired();
            HasOptional(n => n.Sexo).WithMany().Map(n => n.MapKey("SiglaSexo"));
        }
    }
}