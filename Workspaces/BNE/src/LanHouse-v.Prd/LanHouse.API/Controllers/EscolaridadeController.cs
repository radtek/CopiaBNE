using LanHouse.Business.EL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class EscolaridadeController : LanHouse.API.Code.BaseController
    {
        // GET api/<controller>
        public IList Get()
        {
            try
            {
                return Business.Escolaridade.ListarEscolaridadeBNE();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar escolaridades BNE");
                return null;
            }
        }

        public HttpResponseMessage GetGrauEscolaridade(int id)
        {
            try
            {
                string grauEscolaridade = Business.Escolaridade.CarregarGraudaEscolaridade(id);
                return Request.CreateResponse(HttpStatusCode.OK, grauEscolaridade);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Carregar grau da escolaridade");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar grau da escolaridade");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
