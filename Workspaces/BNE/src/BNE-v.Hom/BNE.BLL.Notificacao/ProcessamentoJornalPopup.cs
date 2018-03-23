using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BNE.BLL.Common;
using BNE.BLL.Common.Helpers;
using BNE.BLL.Notificacao.DTO;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
    public class ProcessamentoJornalPopup : ProcessamentoJornalVagas
    {
        protected const string ProcessKeyJornalVagaPopup = "B9U3YAYNRT3FP4VNUGGR";

        public void IniciarProcessoPopup()
        {
            Parallel.ForEach(RecuperarCurriculos(), new ParallelOptions {MaxDegreeOfParallelism = 1}, preCurriculo =>
            {
                CodigoVagas = preCurriculo.Vagas;

                Vagas = JornalService.RecuperarVagas(this);

                var hmtlFacets = FacetsHTML();
                var htmlVagas = VagasHTML();

                var emailDestinatario = preCurriculo.EmlPessoa;
                try
                {
                    if (!string.IsNullOrWhiteSpace(emailDestinatario))
                    {
                        var assunto = string.Format("{0}, separamos vagas no seu perfil! Acesse agora.", Formatting.RetornarPrimeiroNome(preCurriculo.NomePessoa));

                        var linkMaisVagas = string.Empty;

                        if (!string.IsNullOrWhiteSpace(preCurriculo.Cidade) && !string.IsNullOrWhiteSpace(preCurriculo.Funcao) && !string.IsNullOrWhiteSpace(preCurriculo.UF))
                        {
                            linkMaisVagas = string.Format("vagas-de-emprego-para-{0}-em-{1}-{2}", preCurriculo.Funcao.NormalizarURL(), preCurriculo.Cidade.NormalizarURL(), preCurriculo.UF);
                        }

                        var parametros = new Parametro
                        {
                            nome = Formatting.RetornarPrimeiroNome(preCurriculo.NomePessoa),
                            num_vagas = Vagas.Count,
                            url_salavip = GetUrl() + "SalaVIP.aspx" + RecuperarUtm(Enumeradores.UtmUrl.SalaVIP),
                            //link_escolha_plano = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", RecuperarUtm(Enumeradores.UtmUrl.EscolhaPlano)),
                            //link_propaganda = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/vip", RecuperarUtm(Enumeradores.UtmUrl.Propaganda)),
                            url_vagas_no_perfil = string.Format("{0}{1}{2}", GetUrl(), linkMaisVagas, RecuperarUtm(Enumeradores.UtmUrl.MaisVagas)),
                            funcao_cidade = "",
                            qtd_funcao_cidade = 0,
                            facets_funcao_cidade = "",
                            funcao_area = "",
                            qtd_funcao_area = 0,
                            facets_funcao_area = "",
                            link_cadastro = string.Format("{0}cadastro-de-curriculo-gratis?nme={1}&eml={2}&cid={3}-{4}&fun={5}&utm_source=popup-lead&utm_medium=email&utm_campaign=jornaldevaga-popup&utm_term=banner-cadastro", GetUrl(), preCurriculo.NomePessoa.NormalizarURL(), preCurriculo.EmlPessoa, preCurriculo.Cidade, preCurriculo.UF, preCurriculo.Funcao)
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
                            VAGAS = htmlVagas,
                            MAISVAGAS = RecuperarCarta(Enumeradores.Carta.TemplateMaisVagas),
                            PROPAGANDA = RecuperarCarta(Enumeradores.Carta.TemplateTerminarCadastro),
                            FACETS = facets,
                            TAIL = RecuperarCarta(Enumeradores.Carta.TemplateTail),
                            QUEMMEVIU = string.Empty
                        };

                        if (hmtlFacets.QuantidadeFuncaoCidade > 0)
                        {
                            parametros.funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade == 1 ? " Vaga próxima do seu perfil " : " Vagas próximas do seu perfil ";
                            parametros.qtd_funcao_cidade = hmtlFacets.QuantidadeFuncaoCidade;
                            parametros.facets_funcao_cidade = hmtlFacets.ConteudoFuncaoCidade;
                        }

                        if (hmtlFacets.QuantidadeFuncaoArea > 0)
                        {
                            parametros.funcao_area = hmtlFacets.QuantidadeFuncaoArea == 1 ? " Vaga na sua área " : " Vagas na sua área ";
                            parametros.qtd_funcao_area = hmtlFacets.QuantidadeFuncaoArea;
                            parametros.facets_funcao_area = hmtlFacets.ConteudoFuncaoArea;
                        }

                        var html = RecuperarCarta(Enumeradores.Carta.TemplateJornal).Format(parametrosHTML);
                        var mensagem = html.Format(parametros);

                        var utm_source = "jornal-popup";
                        mensagem = mensagem.Format(new {utm_source}).Replace("[url=", GetUrl()).Replace("]", "");

                        EmailService.EnviarEmail(ProcessKeyJornalVagaPopup, Remetente, emailDestinatario, assunto, mensagem);

                        AtualizarEnvio(preCurriculo.IdfPreCadastro, preCurriculo.QtdEmailEnviado);

                        LogEnvioMensagem.Logar(assunto, Remetente, emailDestinatario, null, CodigoVagas, 77);
                    }
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex, "Jornal de Vagas Popup: " + emailDestinatario);
                }
            });
        }

        private IEnumerable<CadastrosPreCurriculo> RecuperarCurriculos()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "alerta.SP_Journal_Popup", null))
            {
                while (dr.Read())
                {
                    yield return new CadastrosPreCurriculo
                    {
                        EmlPessoa = dr["Eml_Pessoa"].ToString(),
                        Vagas = dr["Vagas"].ToString(),
                        NomePessoa = dr["nme_pessoa"].ToString(),
                        IdfPreCadastro = Convert.ToInt32(dr["idf_Pre_Cadastro"]),
                        Cidade = dr["Cidade"].ToString(),
                        UF = dr["UF"].ToString(),
                        Funcao = dr["Funcao"].ToString(),
                        QtdEmailEnviado = Convert.ToInt32(dr["qtd_Mensagens_Enviadas"])
                    };
                }
            }
        }

        private void AtualizarEnvio(int idPreCurriculo, int qtdEmailEnviado)
        {
            var parm = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idf_Pre_Cadastro", SqlDbType = SqlDbType.Int, Value = idPreCurriculo},
                new SqlParameter {ParameterName = "@qtd_EmailEnviados", SqlDbType = SqlDbType.Int, Value = ++qtdEmailEnviado}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "alerta.SP_Journal_Popup_Finalizar", parm);
        }

        private string GetUrl()
        {
            return "http://www.bne.com.br/";
        }
    }
}

#region DTO CadastrosPreCurriculo

public class CadastrosPreCurriculo
{
    public string EmlPessoa { get; set; }
    public string Vagas { get; set; }
    public string NomePessoa { get; set; }
    public int IdfPreCadastro { get; set; }
    public int QtdEmailEnviado { get; set; }
    public string Funcao { get; set; }
    public string Cidade { get; set; }
    public string UF { get; set; }
}

#endregion