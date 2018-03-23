using Bne.Web.Services.API.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// CelularController 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CelularController : ApiController
    {

        /// <summary>
        /// Valida o número de celular do candidato.
        /// </summary>
        /// <param name="param">
        ///     Dados do candidato que será exportado para o BNE.
        /// </param>
        /// <see cref="Bne.Web.Services.API.DTO.Integracao.ExportaCandidatoParam"/>
        /// <returns>Retorna 200 se o celular foi confirmado com sucesso!</returns>
        /// 
        /// <response code="200">Sucesso</response>
        /// <response code="401">Celular não encontrado</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public HttpResponseMessage Validar([FromUri] int Curriculo, [FromUri] string NumCelularCompleto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Celular.Validar(Curriculo, NumCelularCompleto))
                        return Request.CreateResponse(HttpStatusCode.OK);
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ActionContext.ModelState);
            }
        }
    }
}
