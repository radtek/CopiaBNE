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
    public class EscolaridadeEspecializacaoController : ApiController
    {
        // GET: api/EscolaridadeEspecializacao
        public IList Get()
        {
            try
            {
                return Business.Escolaridade.ListarEspecializacoesBNE();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar especializações (escolaridade)");
                return null;
            }
        }

        // GET: api/EscolaridadeEspecializacao/5
        public string Get(int id)
        {
            return "";
        }

        // POST: api/EscolaridadeEspecializacao
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EscolaridadeEspecializacao/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EscolaridadeEspecializacao/5
        public void Delete(int id)
        {
        }
    }
}
