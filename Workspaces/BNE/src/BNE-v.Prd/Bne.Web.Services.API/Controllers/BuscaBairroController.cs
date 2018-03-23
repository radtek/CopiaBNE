using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// BuscaBairroController
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuscaBairroController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Pesquisar(string c, string e, string q)
        {
            try
            {
                var rst = Bairro.ListByCity(c, e, q);
                return Request.CreateResponse(HttpStatusCode.OK, rst);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
