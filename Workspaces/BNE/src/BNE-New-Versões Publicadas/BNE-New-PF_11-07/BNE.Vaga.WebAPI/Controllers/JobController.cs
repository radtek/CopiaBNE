using BNE.Core.Models;
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
    /// <summary>
    /// Implementa os endpoints para as Vagas
    /// </summary>
    public class JobController : ApiController
    {
        #region GET: /Job
        /// <summary>
        /// Obtém uma lista de vagas conforme os parâmetros informados
        /// </summary>
        /// <param name="page">Página a ser recuperada (Iniciando de '1').</param>
        /// <param name="recordsPerPage">Número de Registros por página</param>
        /// <param name="orderBy">Lista de Campos, separados por vírgula, a serem respeitados na ordenação (data, status) </param>
        /// <param name="status">Lista de Status, separados por vírgula (ativa, campanha, pendente, inativa) Segurança: Somente empresas podem filtrar pelo Status "Pendente"</param>
        /// <param name="description">String a ser pesquisada nas propriedades da vaga</param>
        /// <param name="system">Sistema que incluiu a vaga</param>
        /// <param name="group">Grupo da vaga Segurança: Somente empresas logadas podem usar esse parâmetro. Somente Grupos cadastrados na empresa logada podem ser informados.</param>
        /// <param name="title">Função da vaga</param>
        /// <param name="city">Cidade da Vaga (formato: nome-cidade/sigla-estado) </param>
        /// <param name="neighborhood">Bairro</param>
        /// <param name="minSalary">Salário Mínimo da Vaga</param>
        /// <param name="maxSalary">Salário Máximo da Vaga</param>
        /// <param name="onlyFromUsersCompany">Flag indicando se somente as vagas da empresa do usuário devem ser buscadas. Segurança: Somente empresas logadas podem usar esse parâmetro.</param>
        /// <param name="onlyFromUser">Flag indicando se somente as vagas do usuário devem ser buscadas. Segurança: Somente empresas logadas podem usar esse parâmetro.</param>
        /// <param name="importedFrom">Site de onde a vaga foi importada</param>
        /// <returns>Resultado paginado da busca</returns>
        [ResponseType(typeof(PaginatedResult<Models.Job>))]
        [SwaggerResponse(HttpStatusCode.OK, Type=typeof(PaginatedResult<Models.Job>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description="Retornado quando um usuário não logado ou que não seja empresa utilize o parâmetro Group OU um usuário não logado ou que não seja empresa utilize como parâmetro o Status “Pendente”.")]
        public HttpResponseMessage Get( int page, 
                                        int recordsPerPage, 
                                        String orderBy = null, 
                                        String status = null, 
                                        String description = null,
                                        String group = null,
                                        String title = null,
                                        String city = null,
                                        String neighborhood = null,
                                        String system = null,
                                        String importedFrom = null,
                                        decimal? minSalary = null,
                                        decimal? maxSalary = null,
                                        bool onlyFromUsersCompany = false,
                                        bool onlyFromUser = false)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new PaginatedResult<Models.Job>(page, recordsPerPage, 1000, new List<Models.Job>() { new Models.Job(), new Models.Job() }));
        }
        #endregion GET: /Job

        #region GET: /Job/5
        /// <summary>
        /// Obtém uma vaga através do ID informado
        /// </summary>
        /// <param name="id">Id da Vaga a ser retornada</param>
        /// <returns>Vaga referente ao id informado</returns>
        [ResponseType(typeof(Models.Job))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.Job))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Vaga não encontrada para o Id informado")]
        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Models.Job());
        }
        #endregion GET: /Job/5

        #region POST: /Job
        /// <summary>
        /// Inclui uma nova vaga.
        /// </summary>
        /// <param name="job">Vaga a ser incluída.</param>
        /// <returns>Vaga após o salvamento.</returns>
        [ResponseType(typeof(Models.Job))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.Job))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Se o usuário não logado")]
        [SwaggerResponse(HttpStatusCode.Forbidden, Description = "Se o usuário logado mas não é empresa")]
        public HttpResponseMessage Post([FromBody]Models.Job job)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Models.Job());
        }
        #endregion POST: /Job

        #region PUT: /Job/5
        /// <summary>
        /// Atualiza uma vaga
        /// </summary>
        /// <param name="id">Id da vaga a ser atualizada</param>
        /// <param name="job">Vaga com os novos dados</param>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.Job))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Se o usuário não logado")]
        [SwaggerResponse(HttpStatusCode.Forbidden, Description = "Se o usuário logado mas não é empresa OU a vaga não pertence à empresa")]
        public HttpResponseMessage Put(int id, [FromBody]Models.Job job)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Models.Job());
        }
        #endregion PUT: /Job/5

        #region DELETE: /Job/5
        /// <summary>
        /// Inativa a vaga informada
        /// </summary>
        /// <param name="id">Vaga a ser desativada</param>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Se o usuário não logado")]
        [SwaggerResponse(HttpStatusCode.Forbidden, Description = "Se o usuário logado mas não é empresa OU a vaga não pertence à empresa")]
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion DELETE: /Job/5

        public class teste1{
            public string s1;
            public string s2;
        }
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(teste1))]
        public HttpResponseMessage Atualizar(int id, [FromBody] teste1 t)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new teste1());
        }

        public class teste2
        {
            public int s1;
            public bool s2;
        }
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(teste2))]
        public HttpResponseMessage update(int id, [FromBody] teste2 t)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new teste2());
        }

    }
}
