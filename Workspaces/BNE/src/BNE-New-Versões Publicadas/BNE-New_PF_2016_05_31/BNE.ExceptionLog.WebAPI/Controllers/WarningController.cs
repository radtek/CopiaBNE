using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace BNE.ExceptionLog.WebAPI.Controllers
{
    public class WarningController : ApiController
    {
        private readonly Domain.Warning _warning;

        public WarningController(Domain.Warning warning)
        {
            _warning = warning;
        }

        [HttpPost]
        public ResponseMessageResult Post(Models.Warning warning)
        {
            try
            {
                var saved = _warning.Logar(warning.Aplicacao, warning.Usuario, warning.Message, warning.Payload);

                #region SignalR
                var ocorrencias = saved.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia { Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario }).ToList();
                var model = new LogServer.Model.LogInfo(saved.Guid, warning.Aplicacao, warning.Message, ocorrencias, LogServer.Model.LogLevel.Warning);
                //var model = new LogServer.Model.LogInfo(warning.Aplicacao, warning.Message, warning.Payload, saved.Ocorrencias.Count, LogServer.Model.LogLevel.Warning);

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
