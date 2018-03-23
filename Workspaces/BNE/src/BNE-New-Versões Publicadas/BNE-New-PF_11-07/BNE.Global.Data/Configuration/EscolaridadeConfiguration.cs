using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class EscolaridadeConfiguration : EntityTypeConfiguration<Model.EscolaridadeGlobal>
    {
        public EscolaridadeConfiguration()
        {
            ToTable("Escolaridade","global");
            this.HasKey(p=> p.Id);
            this.Property(p=> p.Id).HasColumnName("IdEscolaridade");
            this.Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            this.Property(p => p.DescricaoBNE).HasMaxLength(50).IsRequired();
            this.Property(p => p.Abreviacao).HasMaxLength(8).IsRequired();

            this.HasRequired(p=> p.GrauEscolaridade).WithMany().Map(p=> p.MapKey("IdGrauEscolaridadeGlobal"));
        }
    }
}