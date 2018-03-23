using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain.Custom.Allin;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class IntegradorAllinController : ApiController
    {
        private readonly IntegrarCurriculo _integrarCurriculo;
        private readonly ILogger _logger;

        public IntegradorAllinController(ILogger logger, IntegrarCurriculo integrarCurriculo)
        {
            _integrarCurriculo = integrarCurriculo;
            _logger = logger;
        }


        public ResponseMessageResult Post(int idCurriculo)
        {
            try
            {
                var result = _integrarCurriculo.EnviarDadosAllin(idCurriculo);
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, result));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Integrar CV com Allin.");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
    }
}