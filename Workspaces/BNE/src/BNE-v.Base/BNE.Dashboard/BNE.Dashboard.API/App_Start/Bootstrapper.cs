using Autofac;
using Autofac.Integration.WebApi;
using BNE.Dashboard.Business;
using BNE.Dashboard.Data.Infrastructure;
using System.Web.Http;
using BNE.Dashboard.Data.Repositories;

namespace BNE.Dashboard.API
{
    public static class Bootstrapper
    {
        public static void Configure()
        {
            ConfigureAutofacContainer();
        }

        public static void ConfigureAutofacContainer()
        {

            var webApiContainerBuilder = new ContainerBuilder();
            ConfigureWebApiContainer(webApiContainerBuilder);
        }

        public static void ConfigureWebApiContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces().InstancePerRequest();

            //containerBuilder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerRequest();
            
            //Business Services
            containerBuilder.RegisterType<Business.Watcher>().As<IWatcherService>().InstancePerRequest();
            containerBuilder.RegisterType<Business.GoogleAnalyticsSites>().As<IGoogleAnalyticsSitesService>().InstancePerRequest();

            //Repositories
            containerBuilder.RegisterType<WatcherRepository>().As<IWatcherRepository>().InstancePerRequest();
            containerBuilder.RegisterType<GoogleAnalyticsSitesRepository>().As<IGoogleAnalyticsSitesRepository>().InstancePerRequest();

            //containerBuilder.RegisterType<ResourceRepository>().As<IResourceRepository>().AsImplementedInterfaces().InstancePerApiRequest();
            //containerBuilder.RegisterType<ResourceActivityRepository>().As<IResourceActivityRepository>().InstancePerApiRequest();
            //containerBuilder.RegisterType<LocationRepository>().As<ILocationRepository>().InstancePerApiRequest();
            //containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerApiRequest();
            //containerBuilder.RegisterType<ResourceService>().As<IResourceService>().InstancePerApiRequest();
            //containerBuilder.RegisterType<ResourceActivityService>().As<IResourceActivityService>().InstancePerApiRequest();
            //containerBuilder.RegisterType<LocationService>().As<ILocationService>().InstancePerApiRequest();
            //containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerApiRequest();

            //containerBuilder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ResourceManagerEntities())
            //{
            //    /*Avoids UserStore invoking SaveChanges on every actions.*/
            //    //AutoSaveChanges = false
            //})).As<UserManager<ApplicationUser>>().InstancePerApiRequest();

            containerBuilder.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());
            IContainer container = containerBuilder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}