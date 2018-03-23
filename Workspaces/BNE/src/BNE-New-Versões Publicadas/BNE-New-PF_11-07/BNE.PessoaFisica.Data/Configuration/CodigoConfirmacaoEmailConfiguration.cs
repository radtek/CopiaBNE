using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CodigoConfirmacaoEmailConfiguration : EntityTypeConfiguration<CodigoConfirmacaoEmail>
    {
        public CodigoConfirmacaoEmailConfiguration()
        {
            ToTable("CodigoConfirmacaoEmail", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdCodigoConfirmacaoEmail");
            this.HasKey(p => p.Id);
            this.Property(p => p.DataCriacao).IsRequired();
            this.Property(p => p.DataUtilizacao).IsOptional();
            this.Property(p => p.Email).HasMaxLength(100).IsRequired();
            this.Property(p => p.Codigo).HasMaxLength(100).IsRequired();
        }
    }
}