using BNE.Global.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class RamoAtividadeConfiguration : EntityTypeConfiguration<RamoAtividadeGlobal>
    {
        public RamoAtividadeConfiguration()
        {
            ToTable("RamoAtividade", "global");
            this.Property(p => p.Id).HasColumnName("IdRamoAtividade");
            this.Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}