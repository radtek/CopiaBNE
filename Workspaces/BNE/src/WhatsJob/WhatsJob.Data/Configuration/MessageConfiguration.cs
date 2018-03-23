using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsJob.Data.Configuration
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            ToTable("Message", "WhatsJob");
            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.WhatsId).HasMaxLength(150);
            this.Property(p => p.TextMessage).IsMaxLength();
            this.Property(p => p.Date).IsRequired();
            this.Property(p => p.ReceivedByServer).IsOptional();
            this.Property(p => p.ReceivedByClient).IsOptional();
            this.Property(p => p.ReadByClient).IsOptional();

            this.HasRequired(p => p.WhatsChannel);
            this.HasRequired(p => p.Contact);
        }
    }
}
