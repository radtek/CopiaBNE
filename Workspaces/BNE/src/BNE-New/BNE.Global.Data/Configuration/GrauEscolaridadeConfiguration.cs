using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class GrauEscolaridadeConfiguration : EntityTypeConfiguration<GrauEscolaridadeGlobal>
    {
        public GrauEscolaridadeConfiguration()
        {
            ToTable("GrauEscolaridade", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdGrauEscolaridade");
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}