using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using AutoMapper;
using BNE.PessoaFisica.Domain.Services.SOLR;
using BNE.PessoaFisica.SolrService.Command;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;

namespace BNE.PessoaFisica.SolrService
{
    public class SOLRService : ISOLRService
    {
        //Keep solr job cached for X minutes
        private const int CacheJobMinutes = 60;
        //Keep solr search cached for X minutes
        private const int CacheFullSearchMinutes = 30;

        private readonly ISolrOperations<Model.Vaga> _solrVaga;
        private readonly ISolrQuerySerializer _solrQuerySerializer;
        private readonly SolrQueryExecuter<Model.Vaga> _solrQueryExecuter;


        private readonly IMapper _mapper;
        private readonly Cache.ICachingService _cache;
        public SOLRService(
            ISolrOperations<Model.Vaga> solrVaga,
            IMapper mapper,
            Cache.ICachingService cache,
            ISolrQuerySerializer solrQuerySerializer,
            IIndex<string, ISolrQueryExecuter<Model.Vaga>> serviceDictionary)
        {
            _solrVaga = solrVaga;
            _mapper = mapper;
            _cache = cache;
            _solrQuerySerializer = solrQuerySerializer;

            _solrQueryExecuter = serviceDictionary["JobsSolrNet.Impl.SolrQueryExecuter`1[BNE.PessoaFisica.SolrService.Model.Vaga]"] as SolrQueryExecuter<Model.Vaga>;
        }
        public async Task<List<Vaga>> GetNavigation(int idVaga)
        {
            var job = await GetJobById(idVaga);

            if (job != null)
            {
                var command = new GetJobsCommand
                {
                    IdFuncao = job.IdFuncao,
                    IdCidade = job.IdCidade,
                    Funcao = job.Funcao,
                    Cidade = job.Cidade
                };

                var vagas = await GetJobs(command);

                //return vagas.Select(v => new Vaga
                //{
                //    Id = v.Id,
                //    Url = v.Url.Replace("http://teste.bne.com.br", "http://localhost:51263")
                //}).ToList();

                return _mapper.Map<List<Vaga>>(vagas);
            }
            return null;
        }

        private async Task<Model.Vaga> GetJobById(int idVaga)
        {
            var queryJobById = new SolrQueryByField("id", idVaga.ToString());
            var queryJobByIdRawQuery = _solrQuerySerializer.Serialize(queryJobById);

            var jobResults = _cache.GetItem<SolrQueryResults<Model.Vaga>>(queryJobByIdRawQuery);
            if (jobResults == null)
            {
                jobResults = await _solrVaga.QueryAsync(queryJobById);
                _cache.AddItem(queryJobByIdRawQuery, jobResults, TimeSpan.FromMinutes(CacheJobMinutes));
            }

            return jobResults?.FirstOrDefault();
        }

        public async Task<List<Vaga>> GetNavigation(int idVaga, int idPesquisaVaga, int jobIndex)
        {
            var pesquisaVaga = new BNE.Mapper.ToOld.PesquisaVaga().RecuperarPesquisaVaga(idPesquisaVaga);

            //Se for uma pesquisa por código de vaga a navegação será pelas vagas proximas ao perfil.
            if (!string.IsNullOrWhiteSpace(pesquisaVaga.DescricaoCodVaga))
            {
                return await GetNavigation(idVaga);
            }

            var command = new GetJobsCommand
            {
                IdFuncao = pesquisaVaga.Funcao?.IdFuncao,
                IdCidade = pesquisaVaga.Cidade?.IdCidade,
                Funcao = pesquisaVaga.DescricaoFuncao,
                Cidade = pesquisaVaga.NomeCidade,
                Estado = pesquisaVaga.Cidade?.Estado?.SiglaEstado,
                Escolaridade = (short?)pesquisaVaga.Escolaridade?.IdEscolaridade,
                SalarioMinimo = pesquisaVaga.NumeroSalarioMin,
                SalarioMaximo = pesquisaVaga.NumeroSalarioMax,
                AreaBNE = pesquisaVaga.AreaBNE?.IdAreaBNE,
                PalavraChave = pesquisaVaga.DescricaoPalavraChave,
                RazaoSocial = pesquisaVaga.RazaoSocial,
                Deficiencia = pesquisaVaga.Deficiencia?.IdDeficiencia,
                Disponibilidades = pesquisaVaga.ListarIdentificadoresDisponibilidades(),
                TiposDeVinculo = pesquisaVaga.ListarIdentificadoresTiposDeVinculo(),
                JobIndex = jobIndex
            };

            var vagas = await GetJobs(command);

            var urlSuffix = $"/{idPesquisaVaga}";
            return vagas.Select(v => new Vaga
            {
                Id = v.Id,
                Url = string.Concat(v.Url, urlSuffix) //.Replace("http://teste.bne.com.br", "http://localhost:51263")
            }).ToList();
        }

        private async Task<SolrQueryResults<Model.Vaga>> GetJobs(GetJobsCommand command)
        {
            var filter = new List<ISolrQuery>();
            var orderBy = new List<SortOrder>();
            if (command.IdFuncao.HasValue)
            {
                filter.Add(new SolrQueryByField("Idfs_Funcoes_Sinonimo", command.IdFuncao.ToString()));
                orderBy.Add(new SortOrder($"termfreq(Idf_Funcao, {command.IdFuncao})", Order.DESC));
            }
            if (command.IdCidade.HasValue)
            {
                filter.Add(new SolrQueryByField("Idf_Cidade_Regiao", command.IdCidade.ToString()));
                orderBy.Add(new SortOrder($"termfreq(Idf_Cidade, {command.IdCidade})", Order.DESC));
            }
            if (!string.IsNullOrWhiteSpace(command.Estado))
            {
                filter.Add(new SolrQueryByField("Sig_Estado", command.Estado));
            }
            if (command.Escolaridade.HasValue)
            {
                filter.Add(new SolrQueryByField("Idf_Escolaridade", command.Escolaridade.ToString()));
            }
            if (command.SalarioMinimo.HasValue)
            {
                filter.Add(new SolrQueryByRange<decimal>("Vlr_Salario_De", command.SalarioMinimo.Value, Decimal.MaxValue));
            }
            if (command.SalarioMaximo.HasValue)
            {
                filter.Add(new SolrQueryByRange<decimal>("Vlr_Salario_Para", Decimal.MinValue, command.SalarioMaximo.Value));
            }
            if (command.IdadeMinima.HasValue)
            {
                filter.Add(new SolrQueryByField("Num_Idade_Minima", command.IdadeMinima.ToString()));
            }
            if (command.IdadeMaxima.HasValue)
            {
                filter.Add(new SolrQueryByField("Num_Idade_Maxima", command.IdadeMaxima.ToString()));
            }
            if (command.Sexo.HasValue)
            {
                filter.Add(new SolrQueryByField("Idf_Sexo", command.Sexo.ToString()));
            }
            if (command.AreaBNE.HasValue)
            {
                filter.Add(new SolrQueryByField("Idf_Area_BNE", command.AreaBNE.ToString()));
            }
            if (command.Deficiencia.HasValue)
            {
                filter.Add(new SolrQueryByField("Idf_Deficiencia", command.Deficiencia.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(command.PalavraChave))
            {
                filter.Add(new SolrQuery(command.PalavraChave));
            }
            if (!string.IsNullOrWhiteSpace(command.RazaoSocial))
            {
                filter.Add(new SolrQueryByField("Raz_Social", command.RazaoSocial));
            }
            if (command.Disponibilidades != null && command.Disponibilidades.Any())
            {
                filter.Add(new SolrQueryInList("Idf_Disponibilidade", command.Disponibilidades.Select(c => c.ToString())));
            }
            if (command.TiposDeVinculo != null && command.TiposDeVinculo.Any())
            {
                filter.Add(new SolrQueryInList("Idf_Tipo_Vinculo", command.TiposDeVinculo.Select(c => c.ToString())));
            }

            var extraParams = new Dictionary<string, string> { { "timeAllowed", "1000" } };

            orderBy.Add(new SortOrder("Flg_Vaga_Arquivada", Order.ASC));
            orderBy.Add(new SortOrder("Dta_Abertura", Order.DESC));

            var queryOptions = new QueryOptions
            {
                Fields = new[] { "id", "Url_Vaga" },
                FilterQueries = filter,
                OrderBy = orderBy,
                ExtraParams = extraParams
            };

            if (command.JobIndex.HasValue)
            {
                queryOptions.Rows = 3;
                queryOptions.StartOrCursor = new StartOrCursor.Start(command.JobIndex.Value > 1 ? command.JobIndex.Value - 2 : command.JobIndex.Value - 1);
            }
            else
            {
                queryOptions.Rows = 1000;
            }

            var queryJobByIdRawQuery = string.Join(", ", _solrQueryExecuter.GetAllParameters(SolrQuery.All, queryOptions).Select(p => $"{p.Key}:{p.Value}"));

            var jobResults = _cache.GetItem<SolrQueryResults<Model.Vaga>>(queryJobByIdRawQuery);
            if (jobResults == null)
            {
                jobResults = await _solrVaga.QueryAsync(SolrQuery.All, queryOptions);
                _cache.AddItem(queryJobByIdRawQuery, jobResults, TimeSpan.FromMinutes(CacheFullSearchMinutes));
            }

            return jobResults;
        }
    }
}
