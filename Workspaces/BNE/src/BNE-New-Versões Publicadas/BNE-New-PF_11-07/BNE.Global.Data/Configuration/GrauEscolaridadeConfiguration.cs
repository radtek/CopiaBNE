using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class GrauEscolaridadeConfiguration : EntityTypeConfiguration<Model.GrauEscolaridadeGlobal>
    {
        public GrauEscolaridadeConfiguration()
        {
            ToTable("GrauEscolaridade","global");
            this.HasKey(p=>p.Id);
            this.Property(p => p.Id).HasColumnName("IdGrauEscolaridade");
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}