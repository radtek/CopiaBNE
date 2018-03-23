using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class DeficienciaConfiguration : EntityTypeConfiguration<DeficienciaGlobal>
    {
        public DeficienciaConfiguration()
        {
            ToTable("Deficiencia", "global");
            Property(x => x.Id).HasColumnName("IdDeficiencia");
            HasKey(x => x.Id);
            Property(x => x.Descricao).HasMaxLength(20).IsRequired();
            Property(x => x.CodCaged).IsRequired();
        }
    }
}