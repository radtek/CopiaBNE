using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BNE.Core.Helpers;
using System.Text;

namespace APIGateway.Controllers
{
    public class SwaggerController : ApisController
    {
        /// <summary>
        /// Gets the swagger file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetFile()
        {
            string fileName = Request.GetRouteData().Route.DataTokens["FileName"].ToString();

            if (String.IsNullOrEmpty(fileName))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception("File name not defined in Route Data"));

            String json = Domain.SwaggerConfig.GetFile(fileName);
            if (String.IsNullOrEmpty(json))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new Exception("File specified in Route Data not found in Swagger Files Directory."));

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                //Content = new JsonContent(json)
                Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        }

        [HttpGet]
        public HttpResponseMessage UI(string path)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
