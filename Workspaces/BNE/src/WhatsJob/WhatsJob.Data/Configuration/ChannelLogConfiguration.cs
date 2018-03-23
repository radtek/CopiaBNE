using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;

namespace WhatsJob.Data.Configuration
{
    public class ChannelLogConfiguration : EntityTypeConfiguration<ChannelLog>
    {
        public ChannelLogConfiguration()
        {
            ToTable("ChannelLog", "WhatsJob");
            this.HasKey(p => p.Id);

            this.Property(p => p.Date).IsRequired();
            this.Property(p => p.FaultType).IsRequired();
            this.Property(p => p.Text).IsMaxLength();

            this.HasRequired(p => p.Channel);

        }
    }
}
