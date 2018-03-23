using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class IdiomaConfiguration : EntityTypeConfiguration<Model.IdiomaGlobal>
    {
        public IdiomaConfiguration()
        {
            ToTable("Idioma", "global");
            this.Property(p => p.Id).HasColumnName("IdIdioma");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}