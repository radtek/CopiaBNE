using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class RankingEmailConfiguration : EntityTypeConfiguration<RankingEmail>
    {
        public RankingEmailConfiguration()
        {
            ToTable("RankingEmail", "global");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdRankingEmail");
            Property(n => n.DescricaoEmail).HasMaxLength(30).IsRequired();
        }
    }
}