using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CodigoConfirmacaoEmailConfiguration : EntityTypeConfiguration<CodigoConfirmacaoEmail>
    {
        public CodigoConfirmacaoEmailConfiguration()
        {
            ToTable("CodigoConfirmacaoEmail", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdCodigoConfirmacaoEmail");
            HasKey(p => p.Id);
            Property(p => p.DataCriacao).IsRequired();
            Property(p => p.DataUtilizacao).IsOptional();
            Property(p => p.Email).HasMaxLength(100).IsRequired();
            Property(p => p.Codigo).HasMaxLength(100).IsRequired();
        }
    }
}