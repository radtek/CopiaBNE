using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BNE.ValidaCelular.Model;

namespace BNE.ValidaCelular.Context
{
    internal class ValidaCelularContext : DbContext
    {

        public ValidaCelularContext() : base("ValidaCelular") { }

        public DbSet<CodigoConfirmacaoCelular> CodigoConfirmacaoCelular { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new CodigoConfirmacaoCelularConfiguration());

            modelBuilder.HasDefaultSchema("validacelular");
        }
    }
}
