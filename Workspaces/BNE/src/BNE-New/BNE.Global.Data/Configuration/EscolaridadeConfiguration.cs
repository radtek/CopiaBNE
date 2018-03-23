using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class EscolaridadeConfiguration : EntityTypeConfiguration<EscolaridadeGlobal>
    {
        public EscolaridadeConfiguration()
        {
            ToTable("Escolaridade", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdEscolaridade");
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            Property(p => p.DescricaoBNE).HasMaxLength(50).IsRequired();
            Property(p => p.Abreviacao).HasMaxLength(8).IsRequired();

            HasRequired(p => p.GrauEscolaridade).WithMany().Map(p => p.MapKey("IdGrauEscolaridadeGlobal"));
        }
    }
}