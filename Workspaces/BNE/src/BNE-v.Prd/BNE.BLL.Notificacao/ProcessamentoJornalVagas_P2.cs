using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BNE.BLL.Common;
using BNE.BLL.Common.Helpers;
using BNE.BLL.Notificacao.DTO;
using BNE.BLL.Notificacao.Facets;
using BNE.JornalVagas;
using FormatWith;
using log4net;

namespace BNE.BLL.Notificacao
{
    public partial class ProcessamentoJornalVagas // Tabela: alerta.LOG_Processamento_Jornal_Vagas
    {
        #region IdProcessamentoJornalVagas
        /// <summary>
        ///     Campo obrigatório.
        ///     Campo auto-numerado.
        /// </summary>
        public int IdProcessamentoJornalVagas
        {
            get { return _idProcessamentoJornalVagas; }
            set
            {
                _idProcessamentoJornalVagas = value;
                _modified = true;
            }
        }
        #endregion

        public ProcessingResult Processar(Curriculo curriculo, FacetVaga hmtlFacets, string htmlVagas, int numeroVagas, Dictionary<Enumeradores.Carta, string> cartas, Dictionary<Enumeradores.UtmUrl, string> utms, ILog logger)
        {
            var emailDestinatario = curriculo.Email;
            try
            {
                if (!string.IsNullOrWhiteSpace(emailDestinatario))
                {
                    var stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();

                    var primeiroNome = Formatting.RetornarPrimeiroNome(curriculo.Nome);

                    var assunto = numeroVagas == 1 ? $"{primeiroNome}, temos 1 nova vaga para o seu perfil" : $"{primeiroNome}, temos {numeroVagas} novas vagas para o seu perfil";

                    var utmQuemMeViu = string.Empty;
                    var quantidadeQuemMeViu = 0;
                    var quemmeviu = string.Empty;
                    if (curriculo.QuantidadeQuemMeViu15Dias > 0)
                    {
                        utmQuemMeViu = utms[Enumeradores.UtmUrl.QuemMeViu15];
                        quantidadeQuemMeViu = curriculo.QuantidadeQuemMeViu15Dias;
                        quemmeviu = curriculo.QuantidadeQuemMeViu15Dias == 1 ? "EMPRESA VISUALIZOU SEU CURRÍCULO NOS <strong>ÚLTIMOS 15 DIAS</strong>" : "EMPRESAS VISUALIZARAM SEU CURRÍCULO NOS <strong>ÚLTIMOS 15 DIAS</strong>";
                    }
                    else if (curriculo.QuantidadeQuemMeViu30Dias > 0)
                    {
                        utmQuemMeViu = utms[Enumeradores.UtmUrl.QuemMeViu30];
                        quantidadeQuemMeViu = curriculo.QuantidadeQuemMeViu30Dias;
                        quemmeviu = curriculo.QuantidadeQuemMeViu30Dias == 1 ? "EMPRESA VISUALIZOU SEU CURRÍCULO NOS <strong>ÚLTIMOS 30 DIAS</strong>" : "EMPRESAS VISUALIZARAM SEU CURRÍCULO NOS <strong>ÚLTIMOS 30 DIAS</strong>";
                    }

                    var linkMaisVagas = "/vagas-de-emprego";

                    if (!string.IsNullOrWhiteSpace(curriculo.Funcao) && !string.IsNullOrWhiteSpace(curriculo.Cidade) && !string.IsNullOrWhiteSpace(curriculo.Estado))
                    {
                        linkMaisVagas = $"/vagas-de-emprego-para-{curriculo.Funcao.NormalizarURL()}-em-{curriculo.Cidade.NormalizarURL()}-{curriculo.Estado}";
                    }

                    var parametros = new Parametro
                    {
                        nome = primeiroNome,
                        num_vagas = numeroVagas,
                        url_salavip = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/SalaVIP.aspx", utms[Enumeradores.UtmUrl.SalaVIP]),
                        link_escolha_plano = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", utms[Enumeradores.UtmUrl.EscolhaPlano]),
                        url_vagas_no_perfil = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, linkMaisVagas, utms[Enumeradores.UtmUrl.MaisVagas]),
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
                        facets += cartas[Enumeradores.Carta.TemplateInicioFacet];
                        if (hmtlFacets.QuantidadeFuncaoCidade > 0)
                        {
                            facets += cartas[Enumeradores.Carta.TemplateFacetSimilar];
                        }
                        if (hmtlFacets.QuantidadeFuncaoArea > 0)
                        {
                            facets += cartas[Enumeradores.Carta.TemplateFacetArea];
                        }
                        facets += cartas[Enumeradores.Carta.TemplateFimFacet];
                    }

                    var propaganda = curriculo.VIP ? string.Empty : cartas[Enumeradores.Carta.TemplatePropaganda].FormatWith(new { propagandaUrl = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", utms[Enumeradores.UtmUrl.Propaganda]) });

                    var parametrosHTML = new
                    {
                        TOP = cartas[Enumeradores.Carta.TemplateTop],
                        VAGAS = htmlVagas, //TODO: Validar TratarURL(htmlVagas, curriculo.CPF, curriculo.DataNascimento),
                        MAISVAGAS = cartas[Enumeradores.Carta.TemplateMaisVagas],
                        PROPAGANDA = propaganda,
                        FACETS = facets,
                        TAIL = cartas[Enumeradores.Carta.TemplateTail],
                        QUEMMEVIU = quantidadeQuemMeViu == 0 ? string.Empty : cartas[Enumeradores.Carta.TemplateQuemMeViu]
                    };

                    if (hmtlFacets.QuantidadeFuncaoCidade > 0)
                    {
                        parametros.funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade == 1 ? " Vaga próxima do seu perfil " : " Vagas próximas do seu perfil ";
                        parametros.qtd_funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade;
                        parametros.facets_funcao_cidade = MontarFacets(hmtlFacets.FuncaoCidade, hmtlFacets.TemplateFuncaoCidade, curriculo.CPF, curriculo.DataNascimento);
                    }

                    if (hmtlFacets.QuantidadeFuncaoArea > 0)
                    {
                        parametros.funcao_area = hmtlFacets.QuantidadeFuncaoArea == 1 ? " Vaga na sua área " : " Vagas na sua área ";
                        parametros.qtd_funcao_area = hmtlFacets.QuantidadeFuncaoArea;
                        parametros.facets_funcao_area = MontarFacets(hmtlFacets.FuncaoArea, hmtlFacets.TemplateFuncaoArea, curriculo.CPF, curriculo.DataNascimento);
                    }

                    var html = cartas[Enumeradores.Carta.TemplateJornal].FormatWith(parametrosHTML, MissingKeyBehaviour.Ignore);
                    var mensagem = html.FormatWith(parametros, MissingKeyBehaviour.Ignore).FormatWith(new { utm_source = curriculo.VIP ? "jornal-vip" : "jornal" }, MissingKeyBehaviour.Ignore);

                    stopWatch.Stop();

                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug($"Time processing resume {emailDestinatario}:{stopWatch.Elapsed}");
                    }

                    return new ProcessingResult(assunto, mensagem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Jornal de Vagas: " + emailDestinatario, ex);
            }

            return null;
        }

        private string MontarFacets(List<Facet> facets, string html, decimal cpf, DateTime dataNascimento)
        {
            var sb = new StringBuilder();

            foreach (var facet in facets)
            {
                sb.AppendFormat(html, facet.Funcao, facet.Cidade, facet.SiglaEstado, facet.QuantidadeVaga, facet.QuantidadeVaga > 1 ? "Vagas" : "Vaga", $"{LoginAutomatico.GerarUrl(cpf, dataNascimento, facet.Url)}{facet.Utm}");
            }

            return sb.ToString();
        }

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
            _idfCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
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

        public override string ToString()
        {
            return $"Jornal - {IdProcessamentoJornalVagas}";
        }
    }
}