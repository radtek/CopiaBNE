using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class TipoTelefoneConfiguration : EntityTypeConfiguration<Model.TipoTelefoneGlobal>
    {

        public TipoTelefoneConfiguration()
        {
            ToTable("TipoTelefone", "global");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdTipoTelefone").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(n => n.Descricao).IsRequired().HasMaxLength(20);
        }
    }
}