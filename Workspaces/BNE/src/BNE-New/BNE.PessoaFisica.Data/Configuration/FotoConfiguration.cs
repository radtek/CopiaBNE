using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FotoConfiguration : EntityTypeConfiguration<Foto>
    {
        public FotoConfiguration()
        {
            ToTable("Foto", "pessoafisica");
            Property(p => p.Id).HasColumnName("IdFoto");
            HasKey(p => p.Id);
            Property(p => p.Url).IsRequired();
            Property(p => p.Ativo).IsRequired();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAtualizacao).IsOptional();


            HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
        }
    }
}