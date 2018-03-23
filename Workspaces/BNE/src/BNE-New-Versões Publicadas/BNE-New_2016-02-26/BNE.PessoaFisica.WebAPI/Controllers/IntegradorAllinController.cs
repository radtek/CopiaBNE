using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.PessoaFisica.Domain.Custom.Allin;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class IntegradorAllinController : ApiController
    {
        private readonly ILogger _logger;
        private readonly Domain.Custom.Allin.IntegrarCurriculo _integrarCurriculo;

        public IntegradorAllinController(ILogger logger, IntegrarCurriculo integrarCurriculo)
        {
            _integrarCurriculo = integrarCurriculo;
            _logger = logger;
        }


        public ResponseMessageResult Post(int idCurriculo)
        {
            var result = "";

            try
            {
                result = _integrarCurriculo.EnviarDadosAllin(idCurriculo);
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