using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class DisponibilidadeGlobalConfiguration : EntityTypeConfiguration<DisponibilidadeGlobal>
    {
        public DisponibilidadeGlobalConfiguration()
        {
            ToTable("Disponibilidade", "global");
            Property(p => p.Id).HasColumnName("IdDisponibilidade");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(30).IsRequired();
        }
    }
}