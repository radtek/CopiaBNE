using BNE.Dashboard.Business;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BNE.Dashboard.API.Controllers
{
    public class WatcherController : ApiController
    {
        private readonly IWatcherService _watcherService;

        public WatcherController(IWatcherService watcherService)
        {
            this._watcherService = watcherService;
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                var watchers = _watcherService.GetAll();

                return Request.CreateResponse(HttpStatusCode.OK, watchers);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var watcher = _watcherService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, watcher);
            }
            catch (InstanceNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}