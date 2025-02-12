﻿using BNE.Vaga.WebAPI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BNE.Vaga.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Bootstrapper.Run();

            GlobalConfiguration.Configuration.MessageHandlers.Add(new Comum.Domain.Handlers.TranslationHandler());

        }
    }
}
