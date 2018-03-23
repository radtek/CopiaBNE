using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BNE.Logger.Interface;

namespace BNE.Mensagem.WebAPI.Controllers
{
    public class SMSController : ApiController
    {

        private readonly Domain.SMS _sms;
        private readonly ILogger _logger;

        public SMSController(Domain.SMS sms, ILogger logger)
        {
            _sms = sms;
            _logger = logger;
        }

        public HttpResponseMessage Post(Models.EnviarSMS objSMS, string sistema, string template)
        {
            try
            {
                var command = Mapper.Map<Models.EnviarSMS, Domain.Command.EnviarSMS>(objSMS);
                command.Sistema = sistema;
                command.Template = template;
                //TODO: Não consegui resolver este mapeamento com o automapper
                command.Parametros = objSMS.Parametros;

                var resultado = _sms.EnviarSMS(command);
                if (resultado)
                    return Request.CreateResponse(HttpStatusCode.Created);

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

    }
}