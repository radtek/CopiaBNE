using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.ExceptionLog.LogServer.Helper;

namespace BNE.ExceptionLog.WebAPI.Controllers
{
    public class ErrorController : ApiController
    {
        private readonly Domain.Error _error;

        public ErrorController(Domain.Error error)
        {
            _error = error;
        }

        [HttpPost]
        public ResponseMessageResult Post(Models.Error error)
        {
            try
            {
                AutoMapper.Mapper.CreateMap<Models.Exception, Model.TraceLog>().ForMember(s => s.Data, options => options.Ignore());

                var traceLog = AutoMapper.Mapper.Map<Models.Exception, Model.TraceLog>(error.Exception);

                //Não consegui mapear
                traceLog.Data = error.Exception.Data;

                var saved = _error.Logar(error.Aplicacao, error.Usuario, error.Payload, traceLog, traceLog.Message, error.CustomMessage);

                #region SignalR
                var exceptionDetails = traceLog.DumpException();
                
                var ocorrencias = saved.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia {Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario}).ToList();
                var model = new LogServer.Model.LogInfo(saved.Guid, error.Aplicacao, exceptionDetails, traceLog.Message, error.CustomMessage, ocorrencias);

                LogServer.LogServerManager.Instance.Log(model);
                #endregion

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}
