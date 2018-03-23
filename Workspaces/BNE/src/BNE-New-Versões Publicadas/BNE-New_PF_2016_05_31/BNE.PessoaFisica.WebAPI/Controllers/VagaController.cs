using BNE.ExceptionLog.Interface;
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
        private readonly ILogger _logger;

        public VagaController(Domain.PreCurriculo preCurriculo, ILogger logger)
        {
            _preCurriculo = preCurriculo;
            _logger = logger;
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
                _logger.Error(ex, "Pessoa Fisica API - Carregar Vaga");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }           
        }

        [HttpGet]
        public HttpResponseMessage GetPergunta(string idVaga)
        {
            try
            {
                var result = _preCurriculo.CarregarPergunta(idVaga);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Carregar perguntas da Vaga");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}