using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class DeficienciaConfiguration : EntityTypeConfiguration<Model.DeficienciaGlobal>
    {
        public DeficienciaConfiguration()
        {
            ToTable("Deficiencia", "global");
            this.Property(x => x.Id).HasColumnName("IdDeficiencia");
            this.HasKey(x => x.Id);
            this.Property(x => x.Descricao).HasMaxLength(20).IsRequired();
            this.Property(x => x.CodCaged).IsRequired();
        }
    }
}