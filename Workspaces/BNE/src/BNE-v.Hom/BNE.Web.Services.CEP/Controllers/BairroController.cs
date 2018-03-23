using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.EL;
using BNE.Web.Services.Solr.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using Newtonsoft.Json;

namespace BNE.Web.Services.Solr.Controllers
{

    [RoutePrefix("api/bairro")]
    public class BairroController : ApiController
    {
        [HttpGet]
        [Route("getbyidbairro/{idbairro}")]
        public HttpResponseMessage GetById(int idBairro)
        {
            try
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Bairro>>();

                var parms = new LocalParams 
                { 
                    { "Idf_Bairro", idBairro.ToString() } 
                };

                var filterQuery = parms.Select(p => (ISolrQuery)Query.Field(p.Key).Is(p.Value)).ToList();

                var quantidadeRegistros = 1;

                var bairros = solr.Query(SolrQuery.All, new QueryOptions
                {
                    FilterQueries = filterQuery,
                    Start = 0,
                    Rows = quantidadeRegistros
                });

                var retorno = bairros.Select(b => new { IdBairro = b.IdBairro, Nome = b.NomeBairro }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(retorno));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri]string cidade, [FromUri]string estado, [FromUri]string query, [FromUri]int limit)
        {
            try
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Bairro>>();

                var parms = new LocalParams 
                { 
                    { "Nme_Localidade", cidade }, 
                    { "Sig_Estado", estado }, 
                    { "bairro_text",query } 
                };

                var filterQuery = parms.Select(p => (ISolrQuery)Query.Field(p.Key).Is(p.Value)).ToList();

                var quantidadeRegistros = limit == 0 ? 20 : limit;

                var bairros = solr.Query(SolrQuery.All, new QueryOptions
                {
                    FilterQueries = filterQuery,
                    Start = 0,
                    Rows = quantidadeRegistros
                });

                return Request.CreateResponse(HttpStatusCode.OK, bairros.Select(b => b.NomeBairro).Take((int)limit));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        //[Route("")]
        [Route("getbycidade/{cidade}/{estado}")]
        public HttpResponseMessage GetByCidade(string cidade, string estado)
        {
            try
            {
                var s = new Stopwatch();
                s.Start();

                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Bairro>>();

                var parms = new LocalParams { { "Nme_Localidade", cidade }, { "Sig_Estado", estado } };
                var filterQuery = parms.Select(p => (ISolrQuery)Query.Field(p.Key).Is(p.Value)).ToList();

                var bairros = solr.Query(SolrQuery.All, new QueryOptions
                {
                    FilterQueries = filterQuery,
                    Start = 0
                });

                s.Stop();

                if (bairros.Any(z => z.Zona != null))
                {
                    var zonas = new List<string>();
                    foreach (var zona in bairros.Where(bairro => bairro.Zona != null).SelectMany(bairro => bairro.Zona.Where(zona => !zonas.Contains(zona))))
                    {
                        zonas.Add(zona);
                    }

                    //Montando um dicionairo com os retornos
                    var retorno = zonas.ToDictionary<string, string, IEnumerable<dynamic>>(zona => zona, zona => bairros.Where(z => z.Zona != null && z.Zona.Contains(zona)).OrderBy(b=> b.NomeBairro).Select(b => new { ID = b.IdBairro, Nome = b.NomeBairro }));

                    return Request.CreateResponse(HttpStatusCode.OK, retorno);
                }

                return Request.CreateResponse(HttpStatusCode.OK, bairros.Select(b => new { ID = b.IdBairro, Nome = b.NomeBairro }));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [Route("getbynomebairro/{cidade}/{nomebairro}")]
        public HttpResponseMessage GetByNomeBairro(string cidade, string nomeBairro)
        {
            try
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Bairro>>();

                var parms = new LocalParams 
                { 
                    { "Nme_Localidade", cidade },
                    { "Nme_Bairro", nomeBairro } 
                };

                var filterQuery = parms.Select(p => (ISolrQuery)Query.Field(p.Key).Is(p.Value)).ToList();

                var quantidadeRegistros = 1;

                var bairros = solr.Query(SolrQuery.All, new QueryOptions
                {
                    FilterQueries = filterQuery,
                    Start = 0,
                    Rows = quantidadeRegistros
                });

                var retorno = bairros.Select(b => new { IdBairro = b.IdBairro, Nome = b.NomeBairro }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(retorno));
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
    }
}