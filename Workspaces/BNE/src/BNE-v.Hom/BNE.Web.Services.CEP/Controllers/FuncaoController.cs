using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.EL;
using BNE.Web.Services.Solr.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using FuncaoNaoEncontrada = BNE.Web.Services.Solr.Domain.FuncaoNaoEncontrada;
using SolrNet.DSL;

namespace BNE.Web.Services.Solr.Controllers
{
    [RoutePrefix("api/funcao")]
    public class FuncaoController : BaseController
    {

        private readonly FuncaoNaoEncontrada _funcaoNaoEncontradaDomain;
        private static int MinScore = Convert.ToInt32(ConfigurationManager.AppSettings["MinScoreFuncao"]);

        public FuncaoController(FuncaoNaoEncontrada funcaoNaoEncontradaDomain)
        {
            this._funcaoNaoEncontradaDomain = funcaoNaoEncontradaDomain;
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri]string query, [FromUri]int limit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query)) {
                    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                }

                query = query.Trim();
                string[] words = query.Split(' ');
                query = words.First().ToString() + "^5 " + query.Substring(words.First().Length, (query.Length - words.First().Length)).Trim();
                
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Funcao>>();

                var quantidadeRegistros = limit == 0 ? 50 : limit;

                var funcoes = solr.Query(new SolrQuery(query), new QueryOptions
                {
                    Start = 0,
                    Rows = quantidadeRegistros,
                    ExtraParams = new Dictionary<string, string> { { "wt", "xml" }, { "spellcheck", "false" } }                   
                });

                if (funcoes.Count == 0)
                    _funcaoNaoEncontradaDomain.SalvarFuncaoNaoEncontrada(query, GetIP());

                //return Request.CreateResponse(HttpStatusCode.OK, funcoes.OrderBy(c => c.NomeFuncao).Select(c => c.NomeFuncao).Take((int)limit));
                return Request.CreateResponse(HttpStatusCode.OK, funcoes.Select(c => c.NomeFuncao).Take((int)limit));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

    }
}
