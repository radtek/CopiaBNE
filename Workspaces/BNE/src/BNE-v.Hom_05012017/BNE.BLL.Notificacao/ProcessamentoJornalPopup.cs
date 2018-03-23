using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL.Common;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public class ProcessamentoJornalPopup : ProcessamentoJornalVagas
    {
        #region [Atributos]

        #endregion

        #region [Consulta]

        #endregion

        #region [IniciarProcessoPopup]
          public void IniciarProcessoPopup()
        {

            #region [Recupera a Lista Para Enviar]
		      List<CadastrosPreCurriculo> List = new List<CadastrosPreCurriculo>();
                    using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "alerta.SP_Journal_Popup", null))
                    {
                        while (dr.Read())
                        {
                            CadastrosPreCurriculo objPre = new CadastrosPreCurriculo();
                                objPre.EmlPessoa = dr["Eml_Pessoa"].ToString();
                                objPre.Vagas = dr["Vagas"].ToString();
                                objPre.NomePessoa = dr["nme_pessoa"].ToString();
                                objPre.IdfPreCadastro = Convert.ToInt32(dr["idf_Pre_Cadastro"]);
                                objPre.Cidade = dr["Cidade"].ToString();
                                objPre.UF = dr["UF"].ToString();
                                objPre.Funcao= dr["Funcao"].ToString();
                                objPre.Qtd_EmailEnviado = Convert.ToInt32(dr["qtd_Mensagens_Enviadas"]);
                                List.Add(objPre);
                        }
                    }
	        #endregion

            foreach (var pr in List)
            {
                this.CodigoVagas = pr.Vagas;
                
                var hmtlFacets = FacetsHTML();
                var htmlVagas = VagasHTML();

                var emailDestinatario = pr.EmlPessoa;
                try
                {
                    if (!string.IsNullOrWhiteSpace(emailDestinatario))
                    {

                        var assunto = String.Format("{0}, separamos vagas no seu perfil! Acesse agora.", Common.Helpers.Formatting.RetornarPrimeiroNome(pr.NomePessoa));

                        
                        string linkMaisVagas = string.Empty;
                        
                        if (!String.IsNullOrWhiteSpace(pr.Cidade) && !String.IsNullOrWhiteSpace(pr.Funcao) && !String.IsNullOrWhiteSpace(pr.UF))
                        {
                            linkMaisVagas = String.Format("/vagas-de-emprego-para-{0}-em-{1}-{2}", pr.Funcao.NormalizarURL(), pr.Cidade.NormalizarURL(), pr.UF);
                        }

                        var parametros = new DTO.Parametro
                        {
                            nome = Common.Helpers.Formatting.RetornarPrimeiroNome(pr.NomePessoa),
                            num_vagas = this.Vagas.Count,
                            url_salavip =  GetUrl() + "/SalaVIP.aspx" + base.RecuperarUtm(Enumeradores.UtmUrl.SalaVIP),
                            //link_escolha_plano = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/Payment/PaymentMobile.aspx", RecuperarUtm(Enumeradores.UtmUrl.EscolhaPlano)),
                            //link_propaganda = LoginAutomatico.GerarUrl(curriculo.CPF, curriculo.DataNascimento, "/vip", RecuperarUtm(Enumeradores.UtmUrl.Propaganda)),
                            url_vagas_no_perfil = String.Format("{0}{1}{2}",GetUrl(),linkMaisVagas, RecuperarUtm(Enumeradores.UtmUrl.MaisVagas)),
                            funcao_cidade = "",
                            qtd_funcao_cidade = 0,
                            facets_funcao_cidade = "",
                            funcao_area = "",
                            qtd_funcao_area = 0,
                            facets_funcao_area = "",
                            link_cadastro = String.Format("{0}/cadastro-de-curriculo-gratis?nme={1}&eml={2}&cid={3}-{4}&fun={5}&utm_source=popup-lead&utm_medium=email&utm_campaign=jornaldevaga-popup&utm_term=banner-cadastro",GetUrl(),pr.NomePessoa.NormalizarURL(),pr.EmlPessoa,pr.Cidade,pr.UF,pr.Funcao)
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

                        var utm_source =  "jornal-Popup";
                        mensagem = mensagem.Format(new { utm_source }).Replace("[url=", GetUrl()).Replace("]","");
                        

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

                        AtualizarEnvio(pr.IdfPreCadastro, pr.Qtd_EmailEnviado );
                        //LogEnvioMensagem.Logar(assunto, Remetente, emailDestinatario, curriculo.IdCurriculo, CodigoVagas, 96);

                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Jornal de Vagas Popup: " + emailDestinatario);
                }
            }
        }
        #endregion

        #region [AtualizarEnvio]
        private void AtualizarEnvio(int idPreCurriculo, int qtdEmailEnviado)
        {
            var parm = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@idf_Pre_Cadastro", SqlDbType = SqlDbType.Int, Value = idPreCurriculo},
                new SqlParameter{ParameterName = "@qtd_EmailEnviados", SqlDbType = SqlDbType.Int, Value = ++qtdEmailEnviado}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "alerta.SP_Journal_Popup_Finalizar", parm);
        }
        #endregion

        #region [GetUrl]
        private string GetUrl()
        {
            return "http://www.teste.bne.com.br";
        }
        #endregion

    }
}

#region DTO CadastrosPreCurriculo
public class CadastrosPreCurriculo
{
    public string EmlPessoa { get; set; }
    public string Vagas { get; set; }
    public string NomePessoa { get; set; }
    public int IdfPreCadastro { get; set; }
    public int Qtd_EmailEnviado { get; set; }
    public string Funcao { get; set; }
    public string Cidade { get; set; }
    public string UF { get; set; }

}
#endregion
