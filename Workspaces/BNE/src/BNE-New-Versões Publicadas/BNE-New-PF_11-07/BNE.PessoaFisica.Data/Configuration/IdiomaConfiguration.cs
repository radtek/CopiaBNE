using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class IdiomaConfiguration : EntityTypeConfiguration<Model.Idioma>
    {
        public IdiomaConfiguration()
        {
            ToTable("Idioma", "pessoafisica");
            this.Property(p => p.Id).HasColumnName("IdIdioma");
            this.HasKey(p => p.Id);
            this.Property(p => p.DataCadastro).IsRequired();
            this.Property(p => p.Ativo).IsRequired();

            this.HasRequired(p => p.IdiomaGlobal).WithMany().Map(p => p.MapKey("IdIdiomaGlobal"));
            this.HasRequired(p => p.PessoaFisica).WithMany().Map(p => p.MapKey("IdPessoaFisica"));
            this.HasRequired(p => p.NivelIdioma).WithMany().Map(p => p.MapKey("IdNivelIdioma"));
        }
    }
}