using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class VagaController : ApiController
    {
        private readonly Domain.PreCurriculo _preCurriculo;

        public VagaController(Domain.PreCurriculo preCurriculo)
        {
            _preCurriculo = preCurriculo;
        }

        [HttpGet]
        public HttpResponseMessage Get(string CodigoVaga)
        {
            Domain.Command.Vaga vaga = null;

            try
            {
               vaga = _preCurriculo.CarregarVaga(CodigoVaga);
               return Request.CreateResponse(HttpStatusCode.OK, vaga);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }           
        }

        [HttpGet]
        public HttpResponseMessage GetJaEnviei(int codigoVaga, decimal cpf)
        {
            try
            {
                bool result = _preCurriculo.CandidatoChecarJaEnviei(codigoVaga, cpf);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}