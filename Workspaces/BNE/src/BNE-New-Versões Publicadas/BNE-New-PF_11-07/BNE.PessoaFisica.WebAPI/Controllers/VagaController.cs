using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class VagaController : ApiController
    {
        private readonly ILogger _logger;
        private readonly PreCurriculo _preCurriculo;

        public VagaController(PreCurriculo preCurriculo, ILogger logger)
        {
            _preCurriculo = preCurriculo;
            _logger = logger;
        }

        [HttpGet]
        public HttpResponseMessage Get(string codigoVaga)
        {
            try
            {
                var vaga = _preCurriculo.CarregarVaga(codigoVaga);
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