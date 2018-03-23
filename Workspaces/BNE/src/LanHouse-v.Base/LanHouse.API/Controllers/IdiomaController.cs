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
    public class IdiomaController : LanHouse.API.Code.BaseController
    {
        // GET api/<controller>
        public IList Get()
        {
            try
            {
                return Business.Idioma.ListarIdiomas();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar idiomas");
                return null;
            }
        }
    }
}
