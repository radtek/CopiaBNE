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
    public class SituacaoFormacaoController : LanHouse.API.Code.BaseController
    {
        // GET: api/SituacaoFormacao
        public IList Get()
        {
            try
            {
                return Business.SituacaoFormacao.ListarSituacaoFormacao();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar Situação da Formação");
                return null;
            }
        }

        // GET: api/SituacaoFormacao/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SituacaoFormacao
        public bool Post([FromBody]string value)
        {
            return true;
        }

        // PUT: api/SituacaoFormacao/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SituacaoFormacao/5
        public void Delete(int id)
        {
        }

        
    }
}
