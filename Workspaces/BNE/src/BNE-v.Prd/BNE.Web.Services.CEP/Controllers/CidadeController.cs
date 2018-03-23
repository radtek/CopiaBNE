using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.EL;
using BNE.Web.Services.Solr.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using CidadeNaoEncontrada = BNE.Web.Services.Solr.Domain.CidadeNaoEncontrada;

namespace BNE.Web.Services.Solr.Controllers
{
    [RoutePrefix("api/cidade")]
    public class CidadeController : BaseController
    {

        private readonly CidadeNaoEncontrada _cidadeNaoEncontradaDomain;

        public CidadeController(CidadeNaoEncontrada cidadeNaoEncontradaDomain)
        {
            this._cidadeNaoEncontradaDomain = cidadeNaoEncontradaDomain;
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri]string query, [FromUri]int limit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                }
                
                query = query.Trim();
                
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Cidade>>();

                var quantidadeRegistros = limit == 0 ? 50 : limit;

                var cidades = solr.Query(new SolrQuery(query), new QueryOptions
                {
                    Start = 0,
                    Rows = quantidadeRegistros,
                    ExtraParams = new Dictionary<string, string> { { "wt", "xml" }, { "spellcheck", "false" } }
                });

                if (cidades.Count == 0)
                    _cidadeNaoEncontradaDomain.SalvarCidadeNaoEncontrada(query, GetIP());

                return Request.CreateResponse(HttpStatusCode.OK, cidades.Select(c => c.NomeCidade + "/" + c.SiglaEstado).Take((int)limit));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

    }
}
