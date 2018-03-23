using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// BuscaCEPController 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuscaCEPController : ApiController
    {
        private class PesquisarCEPResult
        {
            public PesquisarCEPResult(string TipoLogradouro, string Logradouro, string Bairro, string Cidade, string Estado)
            {
                this.TipoLogradouro = TipoLogradouro;
                this.Logradouro = Logradouro;
                this.Bairro = Bairro;
                this.Cidade = Cidade;
                this.Estado = Estado;
            }

            public string TipoLogradouro { get; private set; }
            public string Logradouro { get; private set; }
            public string Bairro { get; private set; }
            public string Cidade { get; private set; }
            public string Estado { get; private set; }
        }


        [HttpGet]
        public HttpResponseMessage PesquisarCEP(string NumeroCEP)
        {
            try
            {
                BneCepService.CEPClient client = new BneCepService.CEPClient();
                var cepObj = new BneCepService.CEP() { Cep = NumeroCEP };
                client.CompletarCEP(ref cepObj);

                client.Close();

                if (cepObj.Encontrou)
                {
                    var rst = new PesquisarCEPResult(cepObj.TipoLogradouro, cepObj.Logradouro, cepObj.Bairro, cepObj.Cidade, cepObj.Estado);
                    return Request.CreateResponse(HttpStatusCode.OK, rst);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




    }
}
