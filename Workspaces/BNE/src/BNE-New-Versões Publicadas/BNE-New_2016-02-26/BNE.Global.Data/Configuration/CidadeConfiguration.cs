using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class CidadeConfiguration : EntityTypeConfiguration<Model.Cidade>
    {

        public CidadeConfiguration()
        {
            ToTable("Cidade", "global");
            this.HasKey(n => n.Id);
            this.Property(n => n.Nome).HasMaxLength(50);
            this.HasRequired(n => n.Estado).WithMany(n => n.Cidades).Map(n => n.MapKey("UF"));
        }

    }
}
