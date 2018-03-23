using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace BNE.ExceptionLog.WebAPI.Controllers
{
    public class InformationController : ApiController
    {
        private readonly Domain.Information _information;

        public InformationController(Domain.Information information)
        {
            _information = information;
        }

        [HttpPost]
        public ResponseMessageResult Post(Models.Information information)
        {
            try
            {
                var saved = _information.Logar(information.Aplicacao, information.Usuario, information.Message, information.Payload);

                #region SignalR
                var ocorrencias = saved.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia {Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario}).ToList();
                var model = new LogServer.Model.LogInfo(saved.Guid, information.Aplicacao, information.Message, ocorrencias, LogServer.Model.LogLevel.Information);
                //var model = new LogServer.Model.LogInfo(information.Aplicacao, information.Message, information.Payload, saved.Ocorrencias.Count, LogServer.Model.LogLevel.Information);

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