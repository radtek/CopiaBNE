using LanHouse.Business.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class EmailController : ApiController
    {
        #region EnviarEmail
        [HttpPost]
        public HttpResponseMessage EnviarEmail(string remetente, string destinatario, string assunto, string mensagem)
        {
            try
            {
                MailController.Send(destinatario, remetente, assunto, mensagem);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Enviar Email");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            } 
        }

        #endregion
    }
}
