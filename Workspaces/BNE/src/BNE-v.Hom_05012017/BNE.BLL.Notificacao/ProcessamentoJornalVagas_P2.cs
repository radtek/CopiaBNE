//-- Data: 02/08/2016 16:53
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using BNE.BLL.Common;
using System.Threading.Tasks;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoJornalVagas
    {
        public List<DTO.Vaga> Vagas { get; set; }
        private static int MaxDegreeOfParallelism
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"]); }
        }

        protected static int LimiteEmailEnviarSES
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["LimiteMaximoSES"]); }
        }

        protected const string Remetente = "atendimento@bne.com.br";

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

        /// <summary>
        /// Le uma tabela do banco e retorna os Jornal de Vagas que estão aguardando serem processados
        /// </summary>
        /// <returns>IEnumerable de ProcessamentoJornalVagas</returns>
        public static ConcurrentBag<ProcessamentoJornalVagas> RecuperarJornalAguardandoProcessamento()
        {
            var listaJornalVagas = new ConcurrentBag<ProcessamentoJornalVagas>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarAguardandoProcessamento", null))
            {
                var objProcessamentoJornalVagas = new ProcessamentoJornalVagas();

                while (SetInstance(dr, objProcessamentoJornalVagas, false))
                {
                    listaJornalVagas.Add(objProcessamentoJornalVagas);
                    objProcessamentoJornalVagas = new ProcessamentoJornalVagas();
                }
            }
            return listaJornalVagas;
        }

        public void IniciarProcessamento()
        {
            this.DataInicioProcessamento = DateTime.Now;

            AtualizarDataInicio();
        }

        public void FinalizarProcessamento()
        {
            this.DataFimProcessamento = DateTime.Now;

            AtualizarDataFim();
        }

        private void AtualizarDataInicio()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = this.DataInicioProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataInicioProcessamento", parms);
        }

        private void AtualizarDataFim()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Processamento_Jornal_Vagas", SqlDbType = SqlDbType.Int, Size = 4, Value = IdProcessamentoJornalVagas},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = this.DataFimProcessamento}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarDataFimProcessamento", parms);
        }

        public Facets.Vaga FacetsHTML()
        {
            var parmsFuncaoCidade = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = this._codigoVagas}
            };

            var quantidadeFuncaoCidade = 0;
            var sbFuncaoCidade = new StringBuilder();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_FacetFuncaoCidade", parmsFuncaoCidade))
            {
                while (dr.Read())
                {
                    int quantidade = Convert.ToInt32(dr["Vagas"]);
                    quantidadeFuncaoCidade += quantidade;

                    string funcao = dr["Des_Funcao"].ToString();
                    string cidade = dr["Nme_Cidade"].ToString();
                    string siglaEstado = dr["Sig_Estado"].ToString();
                    var url = string.Format("[url=vagas-de-emprego-para-{0}-em-{1}-{2}]{3}", funcao.NormalizarURL(), cidade.NormalizarURL(), siglaEstado, RecuperarUtm(Enumeradores.UtmUrl.FacetsFuncaoCidade));

                    sbFuncaoCidade.AppendFormat(@"<tr><td bgcolor=""#ffffff"" style=""border-bottom:solid 4px #78909c""><a style=""color:#202020;"" href=""{5}"" style=""color:#202020;""><p style=""font-family: Arial,sans-serif;margin:0;padding:0;color:#757575;font-size:0.8em"">{3} {4} para <strong>{0} em {1}/{2} </strong></p></a></td></tr>", funcao, cidade, siglaEstado, quantidade, quantidade > 1 ? "Vagas" : "Vaga", url);
                }
            }

            var parmsFuncaoArea = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = this._codigoVagas}
            };

            var quantidadeFuncaoArea = 0;
            var sbFuncaoArea = new StringBuilder();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_FacetFuncaoArea", parmsFuncaoArea))
            {
                while (dr.Read())
                {
                    int quantidade = Convert.ToInt32(dr["Vagas"]);
                    quantidadeFuncaoArea += quantidade;

                    string funcao = dr["Des_Funcao"].ToString();
                    string cidade = dr["Nme_Cidade"].ToString();
                    string siglaEstado = dr["Sig_Estado"].ToString();
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
            return this.FlagInvisivel ? UtmsUrlInvisivel[utmUrl] : UtmsUrl[utmUrl];
        }

        protected string RecuperarCarta(Enumeradores.Carta carta)
        {
            return this.FlagInvisivel ? CartasInvisivel[carta] : Cartas[carta];
        }

        public string VagasHTML()
        {
            this.Vagas = RecuperarVagas();

            var sb = new StringBuilder();

            for (int i = 0; i < this.Vagas.Count;)
            {
                DTO.Vaga firstJob = this.Vagas[i];
                DTO.Vaga secondJob = null;
                if (this.Vagas.Count > i + 1)
                {
                    secondJob = this.Vagas[i + 1]; //Pegando o próximo elemento
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
                    if (this.Vagas.Count == 1)
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

        public List<DTO.Vaga> RecuperarVagas()
        {
            var list = new List<DTO.Vaga>();
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_Vagas", SqlDbType = SqlDbType.VarChar, Value = this._codigoVagas}
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarVagas", parms))
            {
                while (dr.Read())
                {
                    var dto = new DTO.Vaga();
                    dto.ReadFromDataReader(dr);
                    list.Add(dto);
                }
            }
            return list;
        }

        public ConcurrentBag<DTO.Curriculo> RecuperarCurriculos()
        {
            var curriculos = new ConcurrentBag<DTO.Curriculo>();
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_curriculos", SqlDbType = SqlDbType.VarChar, Value = this._codigoCurriculos}
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "JornalVaga_RecuperarCurriculos", parms))
            {
                while (dr.Read())
                {
                    var dto = new DTO.Curriculo();

                    dto.ReadFromDataReader(dr);

                    curriculos.Add(dto);
                }
            }

            return curriculos;
        }

        private string DefinirHTMLVaga(DTO.Vaga objVaga)
        {
            if (objVaga.TemPlano)
            {
                if (objVaga.TemDeficiencia)
                    return RecuperarCarta(Enumeradores.Carta.TemplateVagaDeficienteUrgente);
                return RecuperarCarta(Enumeradores.Carta.TemplateVagaUrgente);
            }
            if (objVaga.TemDeficiencia)
                return RecuperarCarta(Enumeradores.Carta.TemplateVagaDeficiente);
            return RecuperarCarta(Enumeradores.Carta.TemplateVagaNormal);
        }

        private DTO.ParametroVaga ParametrosSubstituicao(DTO.Vaga objVaga)
        {
            var atribuicoes = TratarDescricao(objVaga.Atribuicoes);

            return new DTO.ParametroVaga
            {
                nome_vaga = objVaga.DescricaoFuncao,
                salario_vaga = objVaga.Salario,
                cidade_vaga = Common.Helpers.Formatting.FormatarCidade(objVaga.Cidade, objVaga.Estado),
                descricao_vaga = atribuicoes.Length > 140 ? atribuicoes.Substring(0, 140) + "..." : atribuicoes,
                link_vaga = string.Format("[url=vaga-de-emprego-na-area-{0}-em-{1}-{2}/{3}/{4}]{5}", objVaga.Area.NormalizarURL(), objVaga.Cidade.NormalizarURL(), objVaga.Estado, objVaga.DescricaoFuncao.NormalizarURL(), objVaga.IdVaga, RecuperarUtm(Enumeradores.UtmUrl.Vagas)),
                alinhamento = "left"
            };
        }

        private string TratarDescricao(string texto)
        {
            return texto.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "");
        }

        public void Processar()
        {
            IniciarProcessamento();

            var curriculosReceberamAlerta = new List<int>();
            var hmtlFacets = FacetsHTML();
            var htmlVagas = VagasHTML();
            var listaCurriculos = RecuperarCurriculos();

            if (this.Vagas.Count > 0)
            {
                Parallel.ForEach(listaCurriculos, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, curriculo =>
                      {
                          var emailDestinatario = curriculo.Email;
                          try
                          {
                              if (!string.IsNullOrWhiteSpace(emailDestinatario))
                              {

                                  var assunto = this.Vagas.Count == 1 ? String.Format("{0}, temos 1 nova vaga para o seu perfil", Common.Helpers.Formatting.RetornarPrimeiroNome(curriculo.Nome)) : String.Format("{0}, temos {1} novas vagas para o seu perfil", Common.Helpers.Formatting.RetornarPrimeiroNome(curriculo.Nome), this.Vagas.Count);

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

                                  string linkMaisVagas = "/vagas-de-emprego";

                                  if (!string.IsNullOrWhiteSpace(curriculo.Funcao) && !string.IsNullOrWhiteSpace(curriculo.Cidade) && !string.IsNullOrWhiteSpace(curriculo.Estado))
                                  {
                                      linkMaisVagas = string.Format("/vagas-de-emprego-para-{0}-em-{1}-{2}", curriculo.Funcao.NormalizarURL(), curriculo.Cidade.NormalizarURL(), curriculo.Estado);
                                  }

                                  var parametros = new DTO.Parametro
                                  {
                                      nome = Common.Helpers.Formatting.RetornarPrimeiroNome(curriculo.Nome),
                                      num_vagas = this.Vagas.Count,
                                      url_salavip = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/SalaVIP.aspx", RecuperarUtm(Enumeradores.UtmUrl.SalaVIP)),
                                      link_escolha_plano = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", RecuperarUtm(Enumeradores.UtmUrl.EscolhaPlano)),
                                      link_propaganda = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/vip", RecuperarUtm(Enumeradores.UtmUrl.Propaganda)),
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

                                  string facets = string.Empty;
                                  if (hmtlFacets.QuantidadeFuncaoCidade > 0 || hmtlFacets.QuantidadeFuncaoArea > 0)
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
                                      PROPAGANDA = curriculo.VIP ? string.Empty : RecuperarCarta(Enumeradores.Carta.TemplatePropaganda),
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
                                  var mensagem = html.Format(parametros);

                                  var utm_source = curriculo.VIP ? "jornal-vip" : "jornal";
                                  mensagem = mensagem.Format(new { utm_source });

                                  if (this.FlagInvisivel)
                                  {
                                      Email.EnviarSmtpCloudCampanha(assunto, mensagem, Remetente, emailDestinatario);
                                  }
                                  else
                                  {
                                      if (LimiteEmailEnviarSES > AlertaCurriculos.QuantidadeEnviadaHoje())
                                          Email.EnviarAmazonSES(assunto, mensagem, Remetente, emailDestinatario);
                                      else
                                          Email.EnviarSendGrid(assunto, mensagem, Remetente, emailDestinatario);
                                      //Email.EnviarSmtpCloud(assunto, mensagem, Remetente, emailDestinatario);
                                      //MensagemMailing.Enviar(assunto, mensagem, Remetente, emailDestinatario);
                                  }


                                  LogEnvioMensagem.Logar(assunto, Remetente, emailDestinatario, curriculo.IdCurriculo, CodigoVagas, 96);

                                  curriculosReceberamAlerta.Add(curriculo.IdCurriculo);
                              }
                          }
                          catch (Exception ex)
                          {
                              EL.GerenciadorException.GravarExcecao(ex, "Jornal de Vagas: " + emailDestinatario);
                          }
                      }
                );
                AlertaCurriculos.Atualizar(curriculosReceberamAlerta);
            }
            else
            {
                LogErroJornal.Logar("Não veio vaga para processar", this.CodigoCurriculos, this.CodigoVagas);
            }

            FinalizarProcessamento();
        }

        private string TratarURL(string html, decimal cpf, DateTime dataNascimento)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

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
                    html = html.Replace(url, LoginAutomatico.GerarUrl(cpf, dataNascimento, "/" + url.Replace(inicioUrl, string.Empty).Replace(fimUrl, string.Empty)));
                }
            } while (index != -1);


            return html;
        }

        internal static void ProcessarJornaldeVagas()
        {
            while (ExisteAguardandoProcessamento())
            {
                var listaJornal = RecuperarJornalAguardandoProcessamento();

                Parallel.ForEach(listaJornal, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, objProcessamentoJornalVagas =>
                {
                    try
                    {
                        objProcessamentoJornalVagas.Processar();
                    }
                    catch (Exception ex)
                    {
                        var guid = EL.GerenciadorException.GravarExcecao(ex);
                        LogErroJornal.Logar(ex.Message + "Guid: " + guid, objProcessamentoJornalVagas.CodigoCurriculos, objProcessamentoJornalVagas.CodigoVagas);

                        objProcessamentoJornalVagas.FinalizarProcessamento();
                    }
                });

                Thread.Sleep(5000);
            }
        }

        private static bool ExisteAguardandoProcessamento()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "JornalVaga_RecuperarQuantidadeAguardandoProcessamento", null)) > 0;
        }
    }
}