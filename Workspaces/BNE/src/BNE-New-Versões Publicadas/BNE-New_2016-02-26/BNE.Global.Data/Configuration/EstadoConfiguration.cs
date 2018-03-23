using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class EstadoConfiguration : EntityTypeConfiguration<Model.Estado>
    {

        public EstadoConfiguration()
        {
            ToTable("Estado", "global");
            this.HasKey(n => n.UF);
            this.Property(n => n.UF).HasMaxLength(2);
            this.Property(n => n.Nome).HasMaxLength(20).IsRequired();
        }

    }
}
