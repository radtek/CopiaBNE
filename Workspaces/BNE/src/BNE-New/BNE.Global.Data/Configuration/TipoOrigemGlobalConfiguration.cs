using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class TipoOrigemGlobalConfiguration : EntityTypeConfiguration<TipoOrigemGlobal>
    {
        public TipoOrigemGlobalConfiguration()
        {
            ToTable("TipoOrigem", "global");
            Property(p => p.Id).HasColumnName("IdTipoOrigem").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}