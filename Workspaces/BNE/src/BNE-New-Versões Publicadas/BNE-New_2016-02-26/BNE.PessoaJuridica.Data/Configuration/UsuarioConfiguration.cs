using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Model.Usuario>
    {

        public UsuarioConfiguration()
        {
            ToTable("Usuario", "pessoajuridica");
            this.Property(n => n.Nome).HasMaxLength(200).IsRequired();
            this.Property(n => n.DataNascimento).HasColumnType("DATE").IsRequired();
            this.Property(n => n.CPF).HasPrecision(11, 0).IsRequired();
            this.Property(n => n.IP).HasMaxLength(15).IsRequired();
            this.Property(n => n.DataAlteracao).IsRequired();
            this.Property(n => n.DataCadastro).IsRequired();
            this.HasOptional(n => n.Sexo).WithMany().Map(n => n.MapKey("SiglaSexo"));
        }

    }
}
