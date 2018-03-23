using System;
using System.IO;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BNE.PessoaJuridica.ApplicationService.Config;
using log4net;
using Owin;
using SharedKernel.API.Middlewares.Extensions;
using SharedKernel.API.Swagger;
using SharedKernel.API.Swagger.Extensions;
using SharedKernel.Logger;

namespace BNE.PessoaJuridica.WebAPI
{
    public class Startup
    {
        private static readonly ILog log = new LogRepository().GetLogger(typeof(Startup).FullName);
        public void Configuration(IAppBuilder app)
        {
            log.Info("Starting API.");
            try
            {
                var config = new HttpConfiguration();

                config.RegisterSwagger(new SwaggerConfig
                {
                    Title = "Pessoa Juridica API",
                    Version = "v1",
                    XmlCommentsPaths = new[] { @"bin\BNE.PessoaJuridica.WebAPI.XML", @"bin\BNE.PessoaJuridica.ApplicationService.xml" },
                    Description = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "About.txt"))
                });

                var builder = new ContainerBuilder();

                builder.RegisterApiControllers(typeof(Startup).Assembly).InstancePerRequest();
                builder.UseApplicationService();

                var container = builder.Build();
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

                config.Register();

                app.UseCorrelation();
                app.UseAutofacMiddleware(container);
                app.UseAutofacWebApi(config);
                app.UseWebApi(config);

                log.Info("Started API.");
            }
            catch (Exception ex)
            {
                log.Error("Error starting API.", ex);
            }
        }
    }
}