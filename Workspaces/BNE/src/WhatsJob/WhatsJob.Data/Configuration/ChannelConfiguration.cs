using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;

namespace WhatsJob.Data.Configuration
{
    public class ChannelConfiguration : EntityTypeConfiguration<Channel>
    {
        public ChannelConfiguration()
        {
            ToTable("Channel", "WhatsJob");
            this.HasKey(p => p.Number);

            this.Property(p => p.Number).HasMaxLength(150);
            this.Property(p => p.Password).HasMaxLength(100).IsRequired();
            this.Property(p => p.NextChallenge).HasMaxLength(100).IsOptional();
            this.Property(p => p.NickName).HasMaxLength(100).IsRequired();
        }
    }
}
