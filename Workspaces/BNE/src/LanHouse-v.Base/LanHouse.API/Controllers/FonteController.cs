using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class FonteController : LanHouse.API.Code.BaseController
    {
        // GET: api/Fonte
        public IList Get(string query, int limit)
        {
            try
            {
                return Business.Fonte.PesquisarInstituicao(query, limit);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar fontes (instituições)");
                return null;
            }
        }
    }
}
