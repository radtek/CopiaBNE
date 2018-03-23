using Bne.Web.Services.API.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using BNE.BLL.Custom.Email;

namespace Bne.Web.Services.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmailController : ApiController
    {
        //
        // GET: /Email/
        [HttpPost]
        public HttpResponseMessage Send([FromBody] MailMessage message)
        {
            //HttpContext.Current.Request.Headers["Origin"]
            //chrome-extension://hgmloofddffdnphfgcellkdfbfbjeloo

            String Origin = HttpContext.Current.Request.Headers["Origin"];
            List<string> allowedDomains = ConfigurationManager.AppSettings["EmailAllowedDomains"].Split(';').ToList();
            if (allowedDomains.Count(d => Regex.IsMatch(Origin, d)) <= 0)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Acesso não autorizado");

            try
            {
                if (EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(message.subject, message.message, null, message.from, message.to))
                    return Request.CreateResponse(HttpStatusCode.OK, "E-mail enviado com sucesso");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro no envio do e-mail. Por favor, verifique os parâmetros.");
        }

    }
}
