using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain.Custom.Allin;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class IntegradorPreCurriculoAllinController : ApiController
    {
        private readonly IntegrarPreCurriculo _integrarPreCurriculo;
        private readonly RemoverPreCurriculo _removePreCurriculo;
        private readonly ILogger _logger;

        public IntegradorPreCurriculoAllinController(ILogger logger, IntegrarPreCurriculo integrarPreCurriculo, RemoverPreCurriculo removePreCurriculo)
        {
            _integrarPreCurriculo = integrarPreCurriculo;
            _removePreCurriculo = removePreCurriculo;
            _logger = logger;
        }

        public ResponseMessageResult Post(int idPreCurriculo)
        {
            try
            {
                var result = _integrarPreCurriculo.EnviarDadosAllin(idPreCurriculo);
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, result));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Integrar CV com Allin.");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        public ResponseMessageResult RemovePreCurriculo(int idPreCurriculoRemove)
        {
            try
            {
                var result = _removePreCurriculo.EnviarDadosAllin(idPreCurriculoRemove);
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