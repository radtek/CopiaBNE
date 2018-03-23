using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class SexoConfiguration : EntityTypeConfiguration<Model.Sexo>
    {
        public SexoConfiguration()
        {
            ToTable("Sexo", "global");
            this.Property(n => n.Sigla).HasColumnType("char").HasMaxLength(1).IsRequired();
            this.HasKey(n => n.Sigla);
            this.Property(n=>n.Descricao).HasMaxLength(20).IsRequired();
        }
    }
}