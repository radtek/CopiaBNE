using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class TipoTelefoneConfiguration : EntityTypeConfiguration<TipoTelefoneGlobal>
    {
        public TipoTelefoneConfiguration()
        {
            ToTable("TipoTelefone", "global");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdTipoTelefone").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(n => n.Descricao).IsRequired().HasMaxLength(20);
        }
    }
}