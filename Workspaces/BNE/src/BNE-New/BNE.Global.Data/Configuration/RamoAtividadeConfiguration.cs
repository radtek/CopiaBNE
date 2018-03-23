using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class RamoAtividadeConfiguration : EntityTypeConfiguration<RamoAtividadeGlobal>
    {
        public RamoAtividadeConfiguration()
        {
            ToTable("RamoAtividade", "global");
            Property(p => p.Id).HasColumnName("IdRamoAtividade");
            Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}