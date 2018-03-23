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
    public class IntegradorPreCurriculoAllinController : ApiController
    {
         private readonly ILogger _logger;
        private readonly Domain.Custom.Allin.IntegrarPreCurriculo _integrarPreCurriculo;
        private readonly Domain.Custom.Allin.RemoverPreCurriculo _RemovePreCurriculo;

        public IntegradorPreCurriculoAllinController(ILogger logger, IntegrarPreCurriculo integrarPreCurriculo, RemoverPreCurriculo removePreCurriculo)
        {
            _integrarPreCurriculo = integrarPreCurriculo;
            _RemovePreCurriculo = removePreCurriculo;
            _logger = logger;
        }


        public ResponseMessageResult Post(int idPreCurriculo)
        {
            var result = "";

            try
            {
                result = _integrarPreCurriculo.EnviarDadosAllin(idPreCurriculo);
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
            var result = "";

            try
            {
                result = _RemovePreCurriculo.EnviarDadosAllin(idPreCurriculoRemove);
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