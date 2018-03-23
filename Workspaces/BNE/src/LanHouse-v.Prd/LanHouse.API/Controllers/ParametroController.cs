using LanHouse.Business.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{

    public class ParametroController : LanHouse.API.Code.BaseController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                Entities.BNE.TAB_Parametro objParametro = new Business.Parametro().GetById(Convert.ToInt32(Business.Enumeradores.Parametro.SalarioMinimoNacional));
                return Request.CreateResponse(HttpStatusCode.OK, objParametro.Vlr_Parametro);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Parametro");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Parametro");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
