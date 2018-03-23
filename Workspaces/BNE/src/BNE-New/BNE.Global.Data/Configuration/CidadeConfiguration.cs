using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class CidadeConfiguration : EntityTypeConfiguration<Cidade>
    {
        public CidadeConfiguration()
        {
            ToTable("Cidade", "global");
            HasKey(n => n.Id);
            Property(n => n.Nome).HasMaxLength(50);
            HasRequired(n => n.Estado).WithMany(n => n.Cidades).Map(n => n.MapKey("UF"));
        }
    }
}