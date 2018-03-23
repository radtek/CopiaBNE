using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class EmailConfiguration : EntityTypeConfiguration<Email>
    {
        public EmailConfiguration()
        {
            ToTable("Email", "pessoafisica");
            HasKey(p => new {p.Endereco, p.IdPessoaFisica});
            HasRequired(p => p.PessoaFisica).WithMany().HasForeignKey(p => p.IdPessoaFisica);
            Property(p => p.Endereco).HasMaxLength(100).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
        }
    }
}