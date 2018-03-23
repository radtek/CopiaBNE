using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace APIGateway
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger("GatewayAPI");

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new APIGateway.Handlers.ProxyHandler());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = this.Server.GetLastError();
            _logger.Error(ex);
        }
    }
}
