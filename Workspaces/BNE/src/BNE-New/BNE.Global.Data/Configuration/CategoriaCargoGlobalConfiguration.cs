using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class CategoriaCargoGlobalConfiguration : EntityTypeConfiguration<CategoriaCargoGlobal>
    {
        public CategoriaCargoGlobalConfiguration()
        {
            ToTable("CategoriaCargo", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdCategoriaCargo").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}