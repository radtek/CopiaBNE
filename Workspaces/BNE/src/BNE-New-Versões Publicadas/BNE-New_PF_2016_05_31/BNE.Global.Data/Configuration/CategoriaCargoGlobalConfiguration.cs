using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class CategoriaCargoGlobalConfiguration : EntityTypeConfiguration<Model.CategoriaCargoGlobal>
    {
        public CategoriaCargoGlobalConfiguration()
        {
            ToTable("CategoriaCargo", "global");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnName("IdCategoriaCargo").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}