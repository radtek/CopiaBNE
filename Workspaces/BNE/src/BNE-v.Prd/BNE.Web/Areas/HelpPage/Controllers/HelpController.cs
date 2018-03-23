using System;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using BNE.Web.Areas.HelpPage.Models;

namespace BNE.Web.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
#if !DEBUG
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
#endif
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
#if !DEBUG
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
#endif
            
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = HelpPageConfigurationExtensions.GetHelpPageApiModel(Configuration, apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View("Error");
        }
    }
}