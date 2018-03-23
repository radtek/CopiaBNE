using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class BairroConfiguration : EntityTypeConfiguration<Model.Bairro>
    {

        public BairroConfiguration()
        {
            ToTable("Bairro", "global");
            this.HasKey(n => n.Id);
            this.Property(n => n.Nome).HasMaxLength(100);
        }

    }
}
