using WhatsJob.Model;
using System.Data.Entity.ModelConfiguration;


namespace WhatsJob.Data.Configuration
{
    public class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable("Contact", "WhatsJob");
            this.HasKey(p => p.Number);

            this.Property(p => p.Number).HasMaxLength(150);
            this.Property(p => p.From).HasMaxLength(150).IsRequired();
            this.Property(p => p.NickName).HasMaxLength(100).IsRequired();
        }
    }
}
