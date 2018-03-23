using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class SexoConfiguration : EntityTypeConfiguration<Sexo>
    {
        public SexoConfiguration()
        {
            ToTable("Sexo", "global");
            Property(n => n.Sigla).HasColumnType("char").HasMaxLength(1).IsRequired();
            HasKey(n => n.Sigla);
            Property(n => n.Descricao).HasMaxLength(20).IsRequired();
        }
    }
}