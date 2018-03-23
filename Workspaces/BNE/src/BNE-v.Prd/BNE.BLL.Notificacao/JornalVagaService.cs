using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL.Common;
using BNE.BLL.Common.Helpers;
using BNE.BLL.Notificacao.DTO;
using BNE.BLL.Notificacao.Facets;
using BNE.Cache;
using BNE.Infrastructure.Services.SolrService.Contract;
using log4net;

namespace BNE.BLL.Notificacao
{
    public class JornalVagaService
    {
        private static readonly Dictionary<Enumeradores.Carta, string> Cartas = Carta.Recuperar(new List<Enumeradores.Carta>
        {
            Enumeradores.Carta.TemplateJornal,
            Enumeradores.Carta.TemplateVagaNormal,
            Enumeradores.Carta.TemplateVagaUrgente,
            Enumeradores.Carta.TemplateVagaDeficiente,
            Enumeradores.Carta.TemplateVagaDeficienteUrgente,
            Enumeradores.Carta.TemplateInicioFacet,
            Enumeradores.Carta.TemplateFimFacet,
            Enumeradores.Carta.TemplatePropaganda,
            Enumeradores.Carta.TemplateFacetSimilar,
            Enumeradores.Carta.TemplateFacetArea,
            Enumeradores.Carta.TemplateTop,
            Enumeradores.Carta.TemplateMaisVagas,
            Enumeradores.Carta.TemplateQuemMeViu,
            Enumeradores.Carta.TemplateTail,
            Enumeradores.Carta.TemplateTerminarCadastro
        });

        private static readonly Dictionary<Enumeradores.UtmUrl, string> UtmsUrl = UtmUrl.Recuperar(new List<Enumeradores.UtmUrl>
        {
            Enumeradores.UtmUrl.SalaVIP,
            Enumeradores.UtmUrl.EscolhaPlano,
            Enumeradores.UtmUrl.Propaganda,
            Enumeradores.UtmUrl.MaisVagas,
            Enumeradores.UtmUrl.Vagas,
            Enumeradores.UtmUrl.QuemMeViu15,
            Enumeradores.UtmUrl.QuemMeViu30,
            Enumeradores.UtmUrl.FacetsFuncaoArea,
            Enumeradores.UtmUrl.FacetsFuncaoCidade
        });

        private static readonly Dictionary<Enumeradores.Carta, string> CartasInvisivel = CartaInvisivel.Recuperar(new List<Enumeradores.Carta>
        {
            Enumeradores.Carta.TemplateJornal,
            Enumeradores.Carta.TemplateVagaNormal,
            Enumeradores.Carta.TemplateVagaUrgente,
            Enumeradores.Carta.TemplateVagaDeficiente,
            Enumeradores.Carta.TemplateVagaDeficienteUrgente,
            Enumeradores.Carta.TemplateInicioFacet,
            Enumeradores.Carta.TemplateFimFacet,
            Enumeradores.Carta.TemplatePropaganda,
            Enumeradores.Carta.TemplateFacetSimilar,
            Enumeradores.Carta.TemplateFacetArea,
            Enumeradores.Carta.TemplateTop,
            Enumeradores.Carta.TemplateMaisVagas,
            Enumeradores.Carta.TemplateQuemMeViu,
            Enumeradores.Carta.TemplateTail
        });

        private static readonly Dictionary<Enumeradores.UtmUrl, string> UtmsUrlInvisivel = UtmUrlInvisivel.Recuperar(new List<Enumeradores.UtmUrl>
        {
            Enumeradores.UtmUrl.SalaVIP,
            Enumeradores.UtmUrl.EscolhaPlano,
            Enumeradores.UtmUrl.Propaganda,
            Enumeradores.UtmUrl.MaisVagas,
            Enumeradores.UtmUrl.Vagas,
            Enumeradores.UtmUrl.QuemMeViu15,
            Enumeradores.UtmUrl.QuemMeViu30,
            Enumeradores.UtmUrl.FacetsFuncaoArea,
            Enumeradores.UtmUrl.FacetsFuncaoCidade
        });

        private readonly ICachingService _cache;
        private readonly ILog _logger;
        private readonly ISolrService _solrService;

        public JornalVagaService(ILog logger, ICachingService cache, ISolrService solrService)
        {
            _logger = logger;
            _cache = cache;
            _solrService = solrService;
        }

        public JornalVagaService(ILog logger, ICachingService cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IEnumerable<ProcessamentoJornalVagas> GetAllToProcess()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarAguardandoProcessamento", null))
            {
                while (dr.Read())
                {
                    var objProcessamentoJornalVagas = new ProcessamentoJornalVagas();
                    objProcessamentoJornalVagas.SetInstance(dr);
                    yield return objProcessamentoJornalVagas;
                }
            }
        }

        private void AtualizarDataInicio(ProcessamentoJornalVagas objProcessamentoJornalVagas)
        {
            object dataValue = DBNull.Value;
            if (objProcessamentoJornalVagas.DataInicioProcessamento.HasValue)
            {
                dataValue = objProcessamentoJornalVagas.DataInicioProcessamento.Value;
            }

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = dataValue}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataInicioProcessamento", parms);
        }

        private void AtualizarDataFim(ProcessamentoJornalVagas objProcessamentoJornalVagas)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = objProcessamentoJornalVagas.IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = objProcessamentoJornalVagas.DataFimProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataFimProcessamento", parms);
        }

        public async Task<List<Infrastructure.Services.SolrService.Model.Vaga>> RecuperarVagas(ProcessamentoJornalVagas jornal)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var vagas = await _solrService.GetJobsById(jornal.CodigoVagas.Split(',').Where(c => !string.IsNullOrWhiteSpace(c)).ToList());

            stopWatch.Stop();

            if (_logger.IsInfoEnabled)
            {
                _logger.Info($"Tempo recuperando vagas para o {jornal}:{stopWatch.Elapsed}");
            }

            return vagas;
        }

        public Curriculo RecuperarCurriculo(ProcessamentoJornalVagas jornal)
        {
            Curriculo curriculo = null;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = jornal.IdfCurriculo}
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarCurriculo", parms))
            {
                if (dr.Read())
                {
                    curriculo = new Curriculo
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        Nome = dr["Nme_Pessoa"].ToString(),
                        Email = dr["Eml_Pessoa"].ToString(),
                        Funcao = dr["Des_Funcao"].ToString(),
                        Cidade = dr["Nme_Cidade"].ToString(),
                        Estado = dr["Sig_Estado"].ToString(),
                        QuantidadeQuemMeViu15Dias = Convert.ToInt32(dr["QtdQuemMeViu15"]),
                        QuantidadeQuemMeViu30Dias = Convert.ToInt32(dr["QtdQuemMeViu30"]),
                        VIP = Convert.ToBoolean(dr["Flg_VIP"]),
                        CPF = Convert.ToDecimal(dr["Num_CPF"]),
                        DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"])
                    };
                }
            }
            stopWatch.Stop();

            if (_logger.IsInfoEnabled)
            {
                _logger.Info($"Tempo recuperando curriculos para o {jornal}:{stopWatch.Elapsed}");
            }

            return curriculo;
        }

        public void IniciarProcessamento(ProcessamentoJornalVagas jornal)
        {
            jornal.DataInicioProcessamento = DateTime.Now;

            AtualizarDataInicio(jornal);
        }

        public void FinalizarProcessamento(ProcessamentoJornalVagas jornal)
        {
            jornal.DataFimProcessamento = DateTime.Now;

            AtualizarDataFim(jornal);
        }

        public void Reprocessar(ProcessamentoJornalVagas jornal)
        {
            jornal.DataInicioProcessamento = null;

            AtualizarDataInicio(jornal);
        }

        public async Task<FacetVaga> FacetsHTML(List<Infrastructure.Services.SolrService.Model.Vaga> vagas, Dictionary<Enumeradores.UtmUrl, string> utms)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var key = $"facets.jobs:{string.Join("-", vagas.OrderBy(c => c.Id).Select(c => c.Id))}";

            var cached = _cache.GetItem<FacetVaga>(key);
            if (cached != null)
            {
                return cached;
            }

            var facets = new FacetVaga();

            var facetsFuncaoCidade = await _solrService.GetFacetByJobAndCity(vagas);
            foreach (var facet in facetsFuncaoCidade.OrderByDescending(c => c.QuantidadeVagas).Take(5))
            {
                facets.FuncaoCidade.Add(new Facet(facet.QuantidadeVagas, facet.Funcao, facet.Cidade, facet.SiglaEstado, utms[Enumeradores.UtmUrl.FacetsFuncaoCidade]));
            }

            stopWatch.Stop();

            if (_logger.IsInfoEnabled)
            {
                _logger.Info($"Tempo recuperando facets funcao/cidade: {stopWatch.Elapsed}");
            }
            stopWatch.Restart();

            var facetsFuncaoArea = await _solrService.GetFacetByJobAndArea(vagas);
            foreach (var facet in facetsFuncaoArea.OrderByDescending(c => c.QuantidadeVagas).Take(5))
            {
                facets.FuncaoArea.Add(new Facet(facet.QuantidadeVagas, facet.Funcao, facet.Cidade, facet.SiglaEstado, utms[Enumeradores.UtmUrl.FacetsFuncaoArea]));
            }

            stopWatch.Stop();

            if (_logger.IsInfoEnabled)
            {
                _logger.Info($"Tempo recuperando facets funcao/area: {stopWatch.Elapsed}");
            }

            _cache.AddItem(key, facets, TimeSpan.FromHours(3));

            return facets;
        }

        public string VagasHTML(Curriculo objCurriculo, List<Infrastructure.Services.SolrService.Model.Vaga> vagas, Dictionary<Enumeradores.Carta, string> cartas, string utmLinkVaga)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < vagas.Count;)
            {
                var firstJob = vagas[i];
                Infrastructure.Services.SolrService.Model.Vaga secondJob = null;
                if (vagas.Count > i + 1)
                {
                    secondJob = vagas[i + 1]; //Pegando o próximo elemento
                }

                sb.Append(@"<tr><td valign='top' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td valign='top' align='center'><table style='border-collapse:collapse; max-width:600px !important; background-color:#ffffff;' width='100%' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td valign='top' align='center'>");
                
                var firstJobParameters = ParametrosSubstituicao(firstJob, objCurriculo, utmLinkVaga);
                var firstJobTemplate = DefinirHTMLVaga(firstJob, cartas);

                if (secondJob == null)
                {
                    firstJobParameters.alinhamento = "center";
                }

                sb.Append(firstJobTemplate.Format(firstJobParameters));

                if (secondJob != null)
                {
                    var secondJobParameters = ParametrosSubstituicao(secondJob, objCurriculo, utmLinkVaga);
                    var secondJobTemplate = DefinirHTMLVaga(secondJob, cartas);
                    if (vagas.Count == 1)
                    {
                        secondJobParameters.alinhamento = "center";
                    }
                    sb.Append(secondJobTemplate.Format(secondJobParameters));
                }

                sb.Append(@"</td></tr></tbody></table></td></tr></tbody></table></td></tr>");

                i = i + 2; //Pulando uma vaga para pegar a próxima
            }

            return sb.ToString();
        }

        private string DefinirHTMLVaga(Infrastructure.Services.SolrService.Model.Vaga objVaga, Dictionary<Enumeradores.Carta, string> cartas)
        {
            if (objVaga.TemPlano)
            {
                if (objVaga.TemDeficiencia)
                {
                    return cartas[Enumeradores.Carta.TemplateVagaDeficienteUrgente];
                }
                return cartas[Enumeradores.Carta.TemplateVagaUrgente];
            }
            if (objVaga.TemDeficiencia)
            {
                return cartas[Enumeradores.Carta.TemplateVagaDeficiente];
            }
            return cartas[Enumeradores.Carta.TemplateVagaNormal];
        }

        private ParametroVaga ParametrosSubstituicao(Infrastructure.Services.SolrService.Model.Vaga objVaga, Curriculo objCurriculo, string utmLinkVaga)
        {
            var atribuicoes = TratarDescricao(objVaga.Atribuicoes);

            return new ParametroVaga
            {
                nome_vaga = objVaga.DescricaoFuncao,
                salario_vaga = objVaga.Salario,
                cidade_vaga = Formatting.FormatarCidade(objVaga.NomeCidade, objVaga.SiglaEstado),
                descricao_vaga = atribuicoes.Length > 140 ? atribuicoes.Substring(0, 140) + "..." : atribuicoes,
                link_vaga = $"{LoginAutomatico.GerarUrl(objCurriculo.CPF, objCurriculo.DataNascimento, $"/vaga-de-emprego-na-area-{objVaga.Area.NormalizarURL()}-em-{objVaga.NomeCidade.NormalizarURL()}-{objVaga.SiglaEstado}/{objVaga.DescricaoFuncao.NormalizarURL()}/{objVaga.Id}")}{utmLinkVaga}",
                alinhamento = "left",
                codigo_vaga = objVaga.Codigo
            };
        }

        private string TratarDescricao(string texto)
        {
            return texto.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "");
        }

        public Dictionary<Enumeradores.Carta, string> RecuperarCartas(ProcessamentoJornalVagas jornal)
        {
            return jornal.FlagInvisivel ? CartasInvisivel : Cartas;
        }

        public Dictionary<Enumeradores.UtmUrl, string> RecuperarUtms(ProcessamentoJornalVagas jornal)
        {
            return jornal.FlagInvisivel ? UtmsUrlInvisivel : UtmsUrl;
        }
    }
}