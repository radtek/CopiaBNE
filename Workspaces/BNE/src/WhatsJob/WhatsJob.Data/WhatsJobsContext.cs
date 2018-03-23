using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WhatsJob.Data
{
    public class WhatsJobsContext : DbContext
    {
        public WhatsJobsContext() : base("WhatsJobs") { }

        public DbSet<Model.Channel> Channel { get; set; }
        public DbSet<Model.ChannelLog> ChannelLog { get; set; }
        public DbSet<Model.Contact> Contact { get; set; }
        public DbSet<Model.Message> Message { get; set; }
        public DbSet<Model.StopWord> StopWord { get; set; }
        public DbSet<Model.Phrase> Phrase { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Configuration WhatsJob
            modelBuilder.Configurations.Add(new Configuration.ChannelConfiguration());
            modelBuilder.Configurations.Add(new Configuration.ChannelLogConfiguration());
            modelBuilder.Configurations.Add(new Configuration.ContactConfiguration());
            modelBuilder.Configurations.Add(new Configuration.MessageConfiguration());
            modelBuilder.Configurations.Add(new Configuration.StopWordConfiguration());
            modelBuilder.Configurations.Add(new Configuration.PhraseConfiguration());
            #endregion

            //Determina que as colunas string serão Varchar.
            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("Varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

    }
}
