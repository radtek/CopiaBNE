using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BNE.Dashboard.Entities;
using BNE.Dashboard.Data.Configurations;

namespace BNE.Dashboard.Data
{
    public class DashboardEntities : DbContext
    {

        public DashboardEntities() : base("DashboardConnection") { }

        public DbSet<Watcher> Watcher { get; set; }
        public DbSet<MessageQueue> MessageQueue { get; set; }
        public DbSet<WindowsService> WindowsService { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<GoogleAnalyticsSites> GoogleAnalyticsSites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dashboard");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ServiceStatusConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
