using APIGateway.Authentication;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIGateway.Controllers
{
    public class ProxyController : ApiController
    {
        private readonly Domain.Api _api;

        public ProxyController(Domain.Api api)
        {
            _api = api;
        }

        [AcceptVerbs("GET","POST","PUT","DELETE","HEAD","PATCH","OPTIONS")]
        public HttpResponseMessage Forward(string urlSuffix, string relativePath)
        {
            return Request.CreateResponse();
        }

        

        static async Task<HttpResponseMessage> RunAsync(Model.Api api, string relativePath, HttpRequestMessage request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.SendAsync(request);
            }
        }


    }
}
