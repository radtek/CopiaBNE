using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class EstadoConfiguration : EntityTypeConfiguration<Estado>
    {
        public EstadoConfiguration()
        {
            ToTable("Estado", "global");
            HasKey(n => n.UF);
            Property(n => n.UF).HasMaxLength(2);
            Property(n => n.Nome).HasMaxLength(20).IsRequired();
        }
    }
}