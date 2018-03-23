using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class TipoFuncaoGlobalConfiguration : EntityTypeConfiguration<Model.TipoFuncaoGlobal>
    {
        public TipoFuncaoGlobalConfiguration()
        {
            ToTable("TipoFuncao", "global");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnName("IdTipoFuncao").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}