using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class IdiomaConfiguration : EntityTypeConfiguration<IdiomaGlobal>
    {
        public IdiomaConfiguration()
        {
            ToTable("Idioma", "global");
            Property(p => p.Id).HasColumnName("IdIdioma");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}