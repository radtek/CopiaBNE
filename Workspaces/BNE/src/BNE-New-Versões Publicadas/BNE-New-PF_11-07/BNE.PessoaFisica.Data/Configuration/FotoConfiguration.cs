using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FotoConfiguration : EntityTypeConfiguration<Foto>
    {
        public FotoConfiguration()
        {
            ToTable("Foto", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdFoto");
            this.HasKey(p => p.Id);
            this.Property(p => p.Url).IsRequired();
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.DataAtualizacao).IsOptional();


            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p=>p.MapKey("IdPessoaFisica"));
        }
    }
}