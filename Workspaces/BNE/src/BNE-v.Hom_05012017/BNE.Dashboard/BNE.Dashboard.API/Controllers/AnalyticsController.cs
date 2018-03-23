using BNE.Dashboard.Business;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Caching;
using System;
using System.Linq;
using WebApi.OutputCache.V2;
using System.Collections.Generic;

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
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 30)]
        public HttpResponseMessage Get()
        {
            try
            {
                List<Entities.GoogleAnalyticsSites> l;
                string cacheKey = "googleAnalytics";
                if (MemoryCache.Default[cacheKey] != null)
                {
                    l = (List<Entities.GoogleAnalyticsSites>)MemoryCache.Default["googleAnalytics"];
                }
                else
                {
                    l = _googleAnalyticsSitesService.GetAll().ToList();

                    CacheItemPolicy policy = new CacheItemPolicy();

                    policy.AbsoluteExpiration = DateTime.Now.AddSeconds(5);

                    MemoryCache.Default.Add(cacheKey, l, policy);
                }
                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

    }
}
