using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL.Common;
using BNE.BLL.Common.Helpers;
using BNE.BLL.Mensagem;
using BNE.BLL.Notificacao.DTO;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoJornalVagas // Tabela: alerta.LOG_Processamento_Jornal_Vagas
    {
        internal readonly EmailService EmailService = new EmailService();
        internal readonly JornalVagasService JornalService = new JornalVagasService();
        private static readonly int MaxTask = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTask"]);

        protected const string Remetente = "atendimento@bne.com.br";
        protected const string ProcessKeyJornalVagaInvisivel = "ASHY31VWRV1DYX84DNYM";
        protected const string ProcessKeyJornalVaga = "U5IUMGG738Q3NP40BPAZ";

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

        public List<Vaga> Vagas { get; set; }

        public void IniciarProcessamento()
        {
            DataInicioProcessamento = DateTime.Now;

            JornalService.AtualizarDataInicio(this);
        }

        public void FinalizarProcessamento()
        {
            DataFimProcessamento = DateTime.Now;

            JornalService.AtualizarDataFim(this);
        }

        public Facets.Vaga FacetsHTML()
        {
            var parmsFuncaoCidade = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = _codigoVagas}
            };

            var quantidadeFuncaoCidade = 0;
            var sbFuncaoCidade = new StringBuilder();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_FacetFuncaoCidade", parmsFuncaoCidade))
            {
                while (dr.Read())
                {
                    var quantidade = Convert.ToInt32(dr["Vagas"]);
                    quantidadeFuncaoCidade += quantidade;

                    var funcao = dr["Des_Funcao"].ToString();
                    var cidade = dr["Nme_Cidade"].ToString();
                    var siglaEstado = dr["Sig_Estado"].ToString();
                    var url = string.Format("[url=vagas-de-emprego-para-{0}-em-{1}-{2}]{3}", funcao.NormalizarURL(), cidade.NormalizarURL(), siglaEstado, RecuperarUtm(Enumeradores.UtmUrl.FacetsFuncaoCidade));

                    sbFuncaoCidade.AppendFormat(@"<tr><td bgcolor=""#ffffff"" style=""border-bottom:solid 4px #78909c""><a style=""color:#202020;"" href=""{5}"" style=""color:#202020;""><p style=""font-family: Arial,sans-serif;margin:0;padding:0;color:#757575;font-size:0.8em"">{3} {4} para <strong>{0} em {1}/{2} </strong></p></a></td></tr>", funcao, cidade, siglaEstado, quantidade, quantidade > 1 ? "Vagas" : "Vaga", url);
                }
            }

            var parmsFuncaoArea = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = _codigoVagas}
            };

            var quantidadeFuncaoArea = 0;
            var sbFuncaoArea = new StringBuilder();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_FacetFuncaoArea", parmsFuncaoArea))
            {
                while (dr.Read())
                {
                    var quantidade = Convert.ToInt32(dr["Vagas"]);
                    quantidadeFuncaoArea += quantidade;

                    var funcao = dr["Des_Funcao"].ToString();
                    var cidade = dr["Nme_Cidade"].ToString();
                    var siglaEstado = dr["Sig_Estado"].ToString();
                    var url = string.Format("[url=vagas-de-emprego-para-{0}-em-{1}-{2}]{3}", funcao.NormalizarURL(), cidade.NormalizarURL(), siglaEstado, RecuperarUtm(Enumeradores.UtmUrl.FacetsFuncaoArea));

                    sbFuncaoArea.AppendFormat(@"<tr><td bgcolor=""#ffffff"" style=""border-bottom:solid 4px #78909c""><a style=""color:#202020;"" href=""{5}"" style=""color:#202020;""><p style=""font-family: Arial,sans-serif;margin:0;padding:0;color:#757575;font-size:0.8em"">{3} {4} para <strong>{0} em {1}/{2} </strong></p></a></td></tr>", funcao, cidade, siglaEstado, quantidade, quantidade > 1 ? "Vagas" : "Vaga", url);
                }
            }

            return new Facets.Vaga
            {
                QuantidadeFuncaoCidade = quantidadeFuncaoCidade,
                ConteudoFuncaoCidade = sbFuncaoCidade.ToString(),
                QuantidadeFuncaoArea = quantidadeFuncaoArea,
                ConteudoFuncaoArea = sbFuncaoArea.ToString()
            };
        }

        protected string RecuperarUtm(Enumeradores.UtmUrl utmUrl)
        {
            return FlagInvisivel ? UtmsUrlInvisivel[utmUrl] : UtmsUrl[utmUrl];
        }

        protected string RecuperarCarta(Enumeradores.Carta carta)
        {
            return FlagInvisivel ? CartasInvisivel[carta] : Cartas[carta];
        }

        public string VagasHTML()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < Vagas.Count;)
            {
                var firstJob = Vagas[i];
                Vaga secondJob = null;
                if (Vagas.Count > i + 1)
                {
                    secondJob = Vagas[i + 1]; //Pegando o próximo elemento
                }

                sb.Append(@"<tr><td valign='top' align='center'><table width='100%' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td valign='top' align='center'><table style='border-collapse:collapse; max-width:600px !important; background-color:#ffffff;' width='100%' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td valign='top' align='center'>");

                var firstJobParameters = ParametrosSubstituicao(firstJob);
                var firstJobTemplate = DefinirHTMLVaga(firstJob);

                if (secondJob == null)
                {
                    firstJobParameters.alinhamento = "center";
                }

                sb.Append(firstJobTemplate.Format(firstJobParameters));

                if (secondJob != null)
                {
                    var secondJobParameters = ParametrosSubstituicao(secondJob);
                    var secondJobTemplate = DefinirHTMLVaga(secondJob);
                    if (Vagas.Count == 1)
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

        private string DefinirHTMLVaga(Vaga objVaga)
        {
            if (objVaga.TemPlano)
            {
                if (objVaga.TemDeficiencia)
                {
                    return RecuperarCarta(Enumeradores.Carta.TemplateVagaDeficienteUrgente);
                }
                return RecuperarCarta(Enumeradores.Carta.TemplateVagaUrgente);
            }
            if (objVaga.TemDeficiencia)
            {
                return RecuperarCarta(Enumeradores.Carta.TemplateVagaDeficiente);
            }
            return RecuperarCarta(Enumeradores.Carta.TemplateVagaNormal);
        }

        private ParametroVaga ParametrosSubstituicao(Vaga objVaga)
        {
            var atribuicoes = TratarDescricao(objVaga.Atribuicoes);

            return new ParametroVaga
            {
                nome_vaga = objVaga.DescricaoFuncao,
                salario_vaga = objVaga.Salario,
                cidade_vaga = Formatting.FormatarCidade(objVaga.Cidade, objVaga.Estado),
                descricao_vaga = atribuicoes.Length > 140 ? atribuicoes.Substring(0, 140) + "..." : atribuicoes,
                link_vaga = string.Format("[url=vaga-de-emprego-na-area-{0}-em-{1}-{2}/{3}/{4}]{5}", objVaga.Area.NormalizarURL(), objVaga.Cidade.NormalizarURL(), objVaga.Estado, objVaga.DescricaoFuncao.NormalizarURL(), objVaga.IdVaga, RecuperarUtm(Enumeradores.UtmUrl.Vagas)),
                alinhamento = "left",
                codigo_vaga = objVaga.Codigo
            };
        }

        private string TratarDescricao(string texto)
        {
            return texto.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "");
        }

        public void Processar()
        {
            IniciarProcessamento();

            Vagas = JornalService.RecuperarVagas(this);

            if (Vagas.Count > 0)
            {
                var curriculosReceberamAlerta = new ConcurrentBag<int>();

                var hmtlFacets = FacetsHTML();
                var htmlVagas = VagasHTML();

                var tasks = new List<Task>();
                var queue = new BlockingCollection<Curriculo>();

                Task producer = Task.Factory.StartNew(() =>
                {
                    foreach (var item in JornalService.RecuperarCurriculos(this))
                    {
                        queue.Add(item);
                    }

                    queue.CompleteAdding();
                }, TaskCreationOptions.LongRunning);
                tasks.Add(producer);

                for (int x = 0; x < MaxTask; x++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        while (!queue.IsCompleted)
                        {
                            Curriculo item;
                            if (queue.TryTake(out item))
                            {
                                Processar(item, hmtlFacets, htmlVagas, curriculosReceberamAlerta);
                            }
                        }
                    }, TaskCreationOptions.LongRunning));
                }

                Task.WaitAll(tasks.ToArray());

                AlertaCurriculos.Atualizar(curriculosReceberamAlerta);
            }
            else
            {
                LogErroJornal.Logar("Não veio vaga para processar!", CodigoCurriculos, CodigoVagas);
            }

            FinalizarProcessamento();
        }

        private void Processar(Curriculo curriculo, Facets.Vaga hmtlFacets, string htmlVagas, ConcurrentBag<int> curriculosReceberamAlerta)
        {
            var emailDestinatario = curriculo.Email;
            try
            {
                if (!string.IsNullOrWhiteSpace(emailDestinatario))
                {
                    var assunto = Vagas.Count == 1 ? string.Format("{0}, temos 1 nova vaga para o seu perfil", Formatting.RetornarPrimeiroNome(curriculo.Nome)) : string.Format("{0}, temos {1} novas vagas para o seu perfil", Formatting.RetornarPrimeiroNome(curriculo.Nome), Vagas.Count);

                    var utmQuemMeViu = string.Empty;
                    var quantidadeQuemMeViu = 0;
                    var quemmeviu = string.Empty;
                    if (curriculo.QuantidadeQuemMeViu15Dias > 0)
                    {
                        utmQuemMeViu = RecuperarUtm(Enumeradores.UtmUrl.QuemMeViu15);
                        quantidadeQuemMeViu = curriculo.QuantidadeQuemMeViu15Dias;
                        quemmeviu = curriculo.QuantidadeQuemMeViu15Dias == 1 ? "EMPRESA VISUALIZOU SEU CURRÍCULO NOS <strong>ÚLTIMOS 15 DIAS</strong>" : "EMPRESAS VISUALIZARAM SEU CURRÍCULO NOS <strong>ÚLTIMOS 15 DIAS</strong>";
                    }
                    else if (curriculo.QuantidadeQuemMeViu30Dias > 0)
                    {
                        utmQuemMeViu = RecuperarUtm(Enumeradores.UtmUrl.QuemMeViu30);
                        quantidadeQuemMeViu = curriculo.QuantidadeQuemMeViu30Dias;
                        quemmeviu = curriculo.QuantidadeQuemMeViu30Dias == 1 ? "EMPRESA VISUALIZOU SEU CURRÍCULO NOS <strong>ÚLTIMOS 30 DIAS</strong>" : "EMPRESAS VISUALIZARAM SEU CURRÍCULO NOS <strong>ÚLTIMOS 30 DIAS</strong>";
                    }

                    var linkMaisVagas = "/vagas-de-emprego";

                    if (!string.IsNullOrWhiteSpace(curriculo.Funcao) && !string.IsNullOrWhiteSpace(curriculo.Cidade) && !string.IsNullOrWhiteSpace(curriculo.Estado))
                    {
                        linkMaisVagas = string.Format("/vagas-de-emprego-para-{0}-em-{1}-{2}", curriculo.Funcao.NormalizarURL(), curriculo.Cidade.NormalizarURL(), curriculo.Estado);
                    }

                    var parametros = new Parametro
                    {
                        nome = Formatting.RetornarPrimeiroNome(curriculo.Nome),
                        num_vagas = Vagas.Count,
                        url_salavip = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/SalaVIP.aspx", RecuperarUtm(Enumeradores.UtmUrl.SalaVIP)),
                        link_escolha_plano = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", RecuperarUtm(Enumeradores.UtmUrl.EscolhaPlano)),
                        url_vagas_no_perfil = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, linkMaisVagas, RecuperarUtm(Enumeradores.UtmUrl.MaisVagas)),
                        link_quemmeviu = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/QuemMeViuTelaMagica.aspx", utmQuemMeViu),
                        quemmeviu_qtd = quantidadeQuemMeViu,
                        quemmeviu = quemmeviu,
                        funcao_cidade = "",
                        qtd_funcao_cidade = 0,
                        facets_funcao_cidade = "",
                        funcao_area = "",
                        qtd_funcao_area = 0,
                        facets_funcao_area = ""
                    };

                    var facets = string.Empty;
                    if ((hmtlFacets.QuantidadeFuncaoCidade > 0) || (hmtlFacets.QuantidadeFuncaoArea > 0))
                    {
                        facets += RecuperarCarta(Enumeradores.Carta.TemplateInicioFacet);
                        if (hmtlFacets.QuantidadeFuncaoCidade > 0)
                        {
                            facets += RecuperarCarta(Enumeradores.Carta.TemplateFacetSimilar);
                        }
                        if (hmtlFacets.QuantidadeFuncaoArea > 0)
                        {
                            facets += RecuperarCarta(Enumeradores.Carta.TemplateFacetArea);
                        }
                        facets += RecuperarCarta(Enumeradores.Carta.TemplateFimFacet);
                    }

                    var parametrosHTML = new
                    {
                        TOP = RecuperarCarta(Enumeradores.Carta.TemplateTop),
                        VAGAS = TratarURL(htmlVagas, curriculo.CPF, curriculo.DataNascimento),
                        MAISVAGAS = RecuperarCarta(Enumeradores.Carta.TemplateMaisVagas),
                        PROPAGANDA = curriculo.VIP ? string.Empty : TratarURL(RecuperarCarta(Enumeradores.Carta.TemplatePropaganda), curriculo.CPF, curriculo.DataNascimento, RecuperarUtm(Enumeradores.UtmUrl.Propaganda)),
                        FACETS = facets,
                        TAIL = RecuperarCarta(Enumeradores.Carta.TemplateTail),
                        QUEMMEVIU = quantidadeQuemMeViu == 0 ? string.Empty : RecuperarCarta(Enumeradores.Carta.TemplateQuemMeViu)
                    };

                    if (hmtlFacets.QuantidadeFuncaoCidade > 0)
                    {
                        parametros.funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade == 1 ? " Vaga próxima do seu perfil " : " Vagas próximas do seu perfil ";
                        parametros.qtd_funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade;
                        parametros.facets_funcao_cidade = TratarURL(hmtlFacets.ConteudoFuncaoCidade, curriculo.CPF, curriculo.DataNascimento);
                    }

                    if (hmtlFacets.QuantidadeFuncaoArea > 0)
                    {
                        parametros.funcao_area = hmtlFacets.QuantidadeFuncaoArea == 1 ? " Vaga na sua área " : " Vagas na sua área ";
                        parametros.qtd_funcao_area = hmtlFacets.QuantidadeFuncaoArea;
                        parametros.facets_funcao_area = TratarURL(hmtlFacets.ConteudoFuncaoArea, curriculo.CPF, curriculo.DataNascimento);
                    }

                    var html = RecuperarCarta(Enumeradores.Carta.TemplateJornal).Format(parametrosHTML);
                    var mensagem = html.Format(parametros).Format(new { utm_source = curriculo.VIP ? "jornal-vip" : "jornal" });

                    EmailService.EnviarEmail(FlagInvisivel ? ProcessKeyJornalVagaInvisivel : ProcessKeyJornalVaga, Remetente, emailDestinatario, assunto, mensagem);

                    //Retirado, não é mais usado
                    //LogEnvioMensagem.Logar(assunto, Remetente, emailDestinatario, curriculo.IdCurriculo, CodigoVagas, 96);

                    curriculosReceberamAlerta.Add(curriculo.IdCurriculo);
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Jornal de Vagas: " + emailDestinatario);
            }
        }

        private string TratarURL(string html, decimal cpf, DateTime dataNascimento, string utm = null)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            const string inicioUrl = "[url=";
            const string fimUrl = "]";

            int index;
            do
            {
                index = html.IndexOf(inicioUrl, StringComparison.InvariantCultureIgnoreCase);
                if (index > -1)
                {
                    var finalIndex = html.IndexOf(fimUrl, index, StringComparison.InvariantCultureIgnoreCase);
                    var url = html.Substring(index, finalIndex - index + 1);
                    html = html.Replace(url, LoginAutomatico.GerarUrl(cpf, dataNascimento, "/" + url.Replace(inicioUrl, string.Empty).Replace(fimUrl, string.Empty), utm));
                }
            } while (index != -1);


            return html;
        }

        #region Métodos

        #region SetInstance

        /// <summary>
        ///     Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as
        ///     colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool SetInstance(IDataReader dr)
        {
            _idProcessamentoJornalVagas = Convert.ToInt32(dr["Idf_Processamento_Jornal_Vagas"]);
            _codigoVagas = Convert.ToString(dr["Cod_Vagas"]);
            _codigoCurriculos = Convert.ToString(dr["Cod_Curriculos"]);
            _flgInvisivel = Convert.ToBoolean(dr["Flg_Invisivel"]);
            _dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            if (dr["Dta_Inicio_Processamento"] != DBNull.Value)
            {
                _dataInicioProcessamento = Convert.ToDateTime(dr["Dta_Inicio_Processamento"]);
            }
            if (dr["Dta_Fim_Processamento"] != DBNull.Value)
            {
                _dataFimProcessamento = Convert.ToDateTime(dr["Dta_Fim_Processamento"]);
            }

            _persisted = true;
            _modified = false;

            return true;
        }

        #endregion

        #endregion
    }
}