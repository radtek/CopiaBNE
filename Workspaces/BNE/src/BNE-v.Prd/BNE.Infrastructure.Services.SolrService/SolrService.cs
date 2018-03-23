using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using BNE.Cache;
using BNE.Infrastructure.Services.SolrService.Contract;
using BNE.Infrastructure.Services.SolrService.Model;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;

namespace BNE.Infrastructure.Services.SolrService
{
    public class SolrService : ISolrService
    {
        //Keep solr job cached for X minutes
        private const int CacheJobMinutes = 15;
        private const int CacheFacetsMinutes = 240;
        private readonly ISolrOperations<Vaga> _solrVaga;
        private readonly SolrQueryExecuter<Vaga> _solrQueryExecuter;
        private readonly ICachingService _cache;

        public SolrService(ISolrOperations<Vaga> solrVaga, ICachingService cache,
            IIndex<string, ISolrQueryExecuter<Model.Vaga>> serviceDictionary)
        {
            _solrVaga = solrVaga;
            //_solrQuerySerializer = solrQuerySerializer;
            _cache = cache;
            _solrQueryExecuter = serviceDictionary["JobsSolrNet.Impl.SolrQueryExecuter`1[BNE.Infrastructure.Services.SolrService.Model.Vaga]"] as SolrQueryExecuter<Model.Vaga>;
        }


        public async Task<Model.Vaga> GetJobById(int job)
        {
            var cacheKey = $"JOB_{job}";
            var query = new SolrQueryByField("id", job.ToString());

            var cachedJob = _cache.GetItem<Model.Vaga>(cacheKey);
            if (cachedJob == null)
            {
                var solrResult = await _solrVaga.QueryAsync(query);
                cachedJob = solrResult.FirstOrDefault();
                _cache.AddItem(cacheKey, cachedJob, TimeSpan.FromMinutes(CacheJobMinutes));
            }

            return cachedJob;
        }

        public async Task<List<Vaga>> GetJobsById(List<string> jobsIds)
        {
            var jobs = new List<Vaga>();
            var jobsToGetFromSolr = new List<string>();

            //Recuperando vagas do cache
            foreach (var job in jobsIds)
            {
                var cacheKey = $"JOB_{job}";
                var cached = _cache.GetItem<Vaga>(cacheKey);

                //Se a vaga não estiver no cache, buscará do Solr
                if (cached == null)
                {
                    jobsToGetFromSolr.Add(job);
                }
                else
                {
                    jobs.Add(cached);
                }
            }

            if (jobsToGetFromSolr.Count > 0)
            {
                var filter = new List<ISolrQuery> { new SolrQueryInList("id", jobsToGetFromSolr) };

                var queryOptions = new QueryOptions
                {
                    Fields = new[] { "id", "Cod_Vaga", "Flg_Plano", "Des_Funcao", "Flg_Deficiencia", "Des_Area_BNE", "Des_Atribuicoes", "Nme_Cidade", "Sig_Estado", "Idf_Cidade", "Idf_Funcao", "Vlr_Salario_De", "Vlr_Salario_Para", "Vlr_Junior_Funcao_Estado", "Vlr_Master_Funcao_Estado" },
                    FilterQueries = filter
                };

                var solrResult = await _solrVaga.QueryAsync(SolrQuery.All, queryOptions);

                jobs.AddRange(solrResult.ToArray());

                solrResult.ForEach(job =>
                {
                    var cacheKey = $"JOB_{job.Id}";
                    _cache.AddItem(cacheKey, job, TimeSpan.FromMinutes(CacheJobMinutes));
                });
            }

            return jobs;
        }

        public async Task<List<FacetJornal>> GetFacetByJobAndCity(List<Vaga> vagas)
        {
            var filter = new List<ISolrQuery>
            {
                new SolrQueryInList("Idf_Cidade_Regiao", vagas.OrderBy(c => c.IdCidade).Select(c => c.IdCidade).Distinct()),
                new SolrQueryInList("Idfs_Funcoes_Sinonimo", vagas.OrderBy(c => c.IdFuncao).Select(c => c.IdFuncao).Distinct()),
                new SolrQueryByField("Flg_Vaga_Arquivada", "0"),
                new SolrQueryByRange<string>("Dta_Abertura", "*", DateTime.Today.ToString("yyyy-MM-ddT23:59:59.99Z"))
            };

            var queryOptions = new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetPivotQuery { Fields = new[] { new PivotFields("Sig_Estado", "Nme_Cidade", "Des_Funcao") } } }
                },
                FilterQueries = filter
            };

            var queryJobByIdRawQuery = string.Join(", ", _solrQueryExecuter.GetAllParameters(SolrQuery.All, queryOptions).Select(p => $"{p.Key}:{p.Value}"));

            var facets = _cache.GetItem<List<FacetJornal>>(queryJobByIdRawQuery);

            if (facets == null)
            {
                facets = new List<FacetJornal>();

                var solrResult = await _solrVaga.QueryAsync(SolrQuery.All, queryOptions);

                if (solrResult.FacetPivots.Count > 0)
                {
                    foreach (var pivotTable in solrResult.FacetPivots)
                    {
                        foreach (var estado in pivotTable.Value)
                        {
                            foreach (var cidade in estado.ChildPivots)
                            {
                                foreach (var funcao in cidade.ChildPivots)
                                {
                                    facets.Add(new FacetJornal
                                    {
                                        Funcao = funcao.Value,
                                        QuantidadeVagas = funcao.Count,
                                        Cidade = cidade.Value,
                                        SiglaEstado = estado.Value
                                    });
                                }
                            }
                        }
                    }
                }
                _cache.AddItem(queryJobByIdRawQuery, facets, TimeSpan.FromMinutes(CacheFacetsMinutes));
            }

            return facets;
        }

        public async Task<List<FacetJornal>> GetFacetByJobAndArea(List<Vaga> vagas)
        {
            var filter = new List<ISolrQuery>
            {
                new SolrQueryInList("Idf_Cidade_Regiao", vagas.OrderBy(c => c.IdCidade).Select(c => c.IdCidade).Distinct()),
                new SolrQueryInList("Des_Area_BNE", vagas.OrderBy(c => c.Area).Select(c => c.Area).Distinct()),
                new SolrQueryByField("Flg_Vaga_Arquivada", "0"),
                new SolrQueryByRange<string>("Dta_Abertura", "*", DateTime.Today.ToString("yyyy-MM-ddT23:59:59.99Z"))
            };

            var queryOptions = new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetPivotQuery { Fields = new[] { new PivotFields("Sig_Estado", "Nme_Cidade", "Des_Funcao") } } }
                },
                FilterQueries = filter
            };

            var queryJobByIdRawQuery = string.Join(", ", _solrQueryExecuter.GetAllParameters(SolrQuery.All, queryOptions).Select(p => $"{p.Key}:{p.Value}"));

            var facets = _cache.GetItem<List<FacetJornal>>(queryJobByIdRawQuery);

            if (facets == null)
            {
                facets = new List<FacetJornal>();

                var solrResult = await _solrVaga.QueryAsync(SolrQuery.All, queryOptions);

                if (solrResult.FacetPivots.Count > 0)
                {
                    foreach (var pivotTable in solrResult.FacetPivots)
                    {
                        foreach (var estado in pivotTable.Value)
                        {
                            foreach (var cidade in estado.ChildPivots)
                            {
                                foreach (var funcao in cidade.ChildPivots)
                                {
                                    facets.Add(new FacetJornal
                                    {
                                        Funcao = funcao.Value,
                                        QuantidadeVagas = funcao.Count,
                                        Cidade = cidade.Value,
                                        SiglaEstado = estado.Value
                                    });
                                }
                            }
                        }
                    }
                }
                _cache.AddItem(queryJobByIdRawQuery, facets, TimeSpan.FromMinutes(CacheFacetsMinutes));
            }

            return facets;
        }
    }
}