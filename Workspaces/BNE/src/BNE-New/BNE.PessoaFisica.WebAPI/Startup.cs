using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Autofac;
using Autofac.Integration.WebApi;
using BNE.PessoaFisica.ApplicationService.Configuration;
using BNE.PessoaFisica.WebAPI.Controllers;
using log4net;
using Owin;
using SharedKernel.API.Middlewares.Extensions;
using SharedKernel.API.Swagger;
using SharedKernel.API.Swagger.Extensions;
using SharedKernel.Logger;

namespace BNE.PessoaFisica.WebAPI
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
                    Title = "Pessoa Fisica API",
                    Version = "v1",
                    XmlCommentsPaths = new[] { @"bin\BNE.PessoaFisica.WebAPI.XML", @"bin\BNE.PessoaFisica.ApplicationService.xml" },
                    Description = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "About.txt"))
                });


                config.MapHttpAttributeRoutes();

                #region Dependency Injection
                var builder = new ContainerBuilder();

                builder.RegisterApiControllers(typeof(CodigoConfirmacaoEmailController).Assembly).InstancePerRequest();
                builder.UseAutofac();

                var container = builder.Build();
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

                #endregion

                #region Cors, Filter, Routes and Formatters
                config.EnableCors(new EnableCorsAttribute(ConfigurationManager.AppSettings["origins"], "*", "*"));


                config.Formatters.Clear();
                config.Formatters.Add(new JsonMediaTypeFormatter());
                config.BindParameter(typeof(DateTime), new CurrentCultureDateTimeAPI());

                //config.Routes.MapHttpRoute(
                //    name: "DefaultApi",
                //    routeTemplate: "v1/{controller}/{action}/{id}",
                //    defaults: new { id = RouteParameter.Optional }
                //);
                //routeTemplate: "v1/{controller}/{id}",

                #endregion

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
        public class CurrentCultureDateTimeAPI : IModelBinder
        {
            public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                bindingContext.Model = date;
                return true;
            }
        }

    }
}
