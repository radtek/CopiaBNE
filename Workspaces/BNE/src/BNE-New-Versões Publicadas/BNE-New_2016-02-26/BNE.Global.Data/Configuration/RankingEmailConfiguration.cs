using System.Data.Entity.ModelConfiguration;

namespace BNE.Global.Data.Configuration
{
    public class RankingEmailConfiguration : EntityTypeConfiguration<Model.RankingEmail>
    {
        public RankingEmailConfiguration()
        {
            ToTable("RankingEmail", "global");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdRankingEmail");
            this.Property(n => n.DescricaoEmail).HasMaxLength(30).IsRequired();
        }
    }
}