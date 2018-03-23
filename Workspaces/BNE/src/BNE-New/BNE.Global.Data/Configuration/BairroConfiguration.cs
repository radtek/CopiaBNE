using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class BairroConfiguration : EntityTypeConfiguration<Bairro>
    {
        public BairroConfiguration()
        {
            ToTable("Bairro", "global");
            HasKey(n => n.Id);
            Property(n => n.Nome).HasMaxLength(100);
        }
    }
}