using BNE.Global.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class DisponibilidadeGlobalConfiguration : EntityTypeConfiguration<DisponibilidadeGlobal>
    {
        public DisponibilidadeGlobalConfiguration()
        {
            ToTable("Disponibilidade", "global");
            this.Property(p => p.Id).HasColumnName("IdDisponibilidade");
            this.HasKey(p => p.Id);
            this.Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}