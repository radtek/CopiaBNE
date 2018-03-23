using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class EmailConfiguration : EntityTypeConfiguration<Model.Email>
    {
        public EmailConfiguration()
        {
            ToTable("Email","pessoafisica");
            this.HasKey(p => new { p.Endereco, p.IdPessoaFisica });
            this.HasRequired(p=>p.PessoaFisica).WithMany().HasForeignKey(p=>p.IdPessoaFisica);
            this.Property(p=>p.Endereco).HasMaxLength(100).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
        }
    }
}