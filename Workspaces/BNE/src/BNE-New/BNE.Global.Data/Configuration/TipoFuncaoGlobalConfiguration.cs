using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class TipoFuncaoGlobalConfiguration : EntityTypeConfiguration<TipoFuncaoGlobal>
    {
        public TipoFuncaoGlobalConfiguration()
        {
            ToTable("TipoFuncao", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdTipoFuncao").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}