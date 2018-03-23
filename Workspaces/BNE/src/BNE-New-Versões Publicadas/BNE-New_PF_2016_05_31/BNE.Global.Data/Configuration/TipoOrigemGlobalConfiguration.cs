using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class TipoOrigemGlobalConfiguration : EntityTypeConfiguration<Model.TipoOrigemGlobal>
    {
        public TipoOrigemGlobalConfiguration()
        {
            ToTable("TipoOrigem", "global");
            this.Property(p => p.Id).HasColumnName("IdTipoOrigem").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}