using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;

namespace WhatsJob.Data.Configuration
{
    public class StopWordConfiguration: EntityTypeConfiguration<StopWord>
    {
        public StopWordConfiguration()
        {
            ToTable("StopWord", "WhatsJob");
            this.HasKey(p => p.Word);
        }
    }
}
