using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class EscolaridadeWEController : LanHouse.API.Code.BaseController
    {

        // GET api/<controller>
        [HttpGet]
        public IList ListarIncompletas()
        {
            try
            {
                return Business.Escolaridade.ListarEscolaridadesIncompletas();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - ListarEscolaridadesIncompletas()");
                return null;
            }
        }


        [HttpGet]
        public IList CarregarMaiorIncompleta(int id)
        {
            try
            {
                return Business.Escolaridade.CarregarEscolaridadesIncompletas(id);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - CarregarMaiorEscolaridadeIncompleta(id)");
                return null;
            }
        }

    }
}
