using BNE.Logger.Interface;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Threading;
using BNE.Logger;

namespace BNE.Vaga.Data
{
    public class VagaContext : DbContext
    {
        private static readonly ILogger _logger = new DatabaseLogger();

        public VagaContext()
            : base("name=Vaga")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            //((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ObjectContext_ObjectMaterialized;
        }

        //protected void ObjectContext_ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        //{
        //    if (e.Entity == null)
        //        return;

        //    if (e.Entity is Comum.Model.Localizable.ILocalizableEntity)
        //    {
        //        //Thread.CurrentThread.CurrentCulture.
        //        BNE.Comum.Model.Localizable.Translator.Translate(e.Entity, this);
        //    }
        //}
        
        public DbSet<Model.Beneficio> Beneficio { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            #region Configurations
            modelBuilder.Configurations.Add(new Configuration.BeneficioConfiguration());
            modelBuilder.Configurations.Add(new BNE.Comum.Data.Configuration.TranslationConfiguration("vaga"));
            //modelBuilder.Configurations.Add(new Configuration.LocalizableEntityConfiguration());
            //modelBuilder.Configurations.Add(new Configuration.LocalizableTranslationConfiguration());
            #endregion

            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("varchar"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                _logger.Error(ex);
                throw (ex);
            }
        }
    }
}
