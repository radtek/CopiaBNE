using BNE.Dashboard.Business;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BNE.Dashboard.API.Controllers
{
    public class AnalyticsController : ApiController
    {
        private readonly IGoogleAnalyticsSitesService _googleAnalyticsSitesService;

        public AnalyticsController(IGoogleAnalyticsSitesService googleAnalyticsSitesService)
        {
            this._googleAnalyticsSitesService = googleAnalyticsSitesService;
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                var sites = _googleAnalyticsSitesService.GetAll();

                return Request.CreateResponse(HttpStatusCode.OK, sites);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

    }
}
