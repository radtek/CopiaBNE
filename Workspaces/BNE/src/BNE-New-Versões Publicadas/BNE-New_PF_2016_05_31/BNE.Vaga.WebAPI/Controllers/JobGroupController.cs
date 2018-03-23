using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BNE.Vaga.WebAPI.Controllers
{
    public class JobGroupController : ApiController
    {
        // GET: /JobGroup
        /// <summary>
        /// Obtém a lista de benefícios de vagas
        /// </summary>
        /// <param name="suffix">Considerado para busca parcial de termos nas descrições dos benefícios</param>
        /// <param name="pessoaJuridica">Guid da pessoa física a ser considerado. Se informado listará os benefícios padrão mais os benefícios incluídos pela empresa.</param>
        /// <param name="listAll">Lista todos os benefícios sendo eles default ou adicionados por empresas</param>
        /// <returns>Lista de benefícios ordenados em ordem alfabética</returns>
        [ResponseType(typeof(List<String>))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<String>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Retornado quando um usuário não logado ou que não seja empresa utilize o parâmetro 'companyId'.")]
        public HttpResponseMessage Get([FromUri] String suffix = null, [FromUri]Guid? companyId = null, [FromUri]bool listAll = false, [FromUri]String language = "pt-BR")
        {
            return Request.CreateResponse(HttpStatusCode.OK, new List<String> { "Vale Alimentação", "Vale Refeição", "Vale Transporte", "Comissão" });
        }

        // GET: /JobGroup/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: /JobGroup
        public void Post([FromBody]string value)
        {
        }

        // PUT: /JobGroup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: /JobGroup/5
        public void Delete(int id)
        {
        }
    }
}
