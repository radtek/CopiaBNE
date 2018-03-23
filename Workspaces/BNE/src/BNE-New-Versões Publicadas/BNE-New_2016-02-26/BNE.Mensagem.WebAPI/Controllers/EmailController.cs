using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BNE.ExceptionLog.Interface;
using BNE.Mensagem.Domain.Exceptions;

namespace BNE.Mensagem.WebAPI.Controllers
{
    public class EmailController : ApiController
    {

        private readonly Domain.Email _email;
        private readonly ILogger _logger;

        public EmailController(Domain.Email email, ILogger logger)
        {
            _email = email;
            _logger = logger;
        }

        public HttpResponseMessage Post(Models.EnviarEmail objEmail, string sistema, string template)
        {
            try
            {
                var command = Mapper.Map<Models.EnviarEmail, Domain.Command.EnviarEmail>(objEmail);
                command.Sistema = sistema;
                command.Template = template;
                //TODO: Não consegui resolver este mapeamento com o automapper
                command.Parametros = objEmail.Parametros;

                var resultado = _email.EnviarEmail(command);
                if (resultado)
                    return Request.CreateResponse(HttpStatusCode.Created);

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                if (ex is SemParametroException)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}