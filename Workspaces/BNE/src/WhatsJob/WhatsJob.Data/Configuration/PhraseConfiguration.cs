using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;

namespace WhatsJob.Data.Configuration
{
    public class PhraseConfiguration : EntityTypeConfiguration<Phrase>
    {
        public PhraseConfiguration()
        {
            ToTable("Phrase", "WhatsJob");
            this.HasKey(p => p.Description);

            this.Property(p => p.Description).HasMaxLength(250);
        }
    }
}
