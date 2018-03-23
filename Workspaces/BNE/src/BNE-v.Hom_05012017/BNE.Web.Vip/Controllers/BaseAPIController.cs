using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BNE.Web.Vip.Controllers
{
    public class BaseAPIController : ApiController
    {
        [AllowAnonymous]
        [Route("sair")]
        [Route("logout")]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Sair()
        {

            BNE.Auth.BNEAutenticacao.DeslogarPadrao();
            var url = string.Concat("http://", BNE.BLL.Custom.Helper.RecuperarURLAmbiente(), "/login.aspx?Deslogar=true");
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(url);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
