using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BNE.BaseService;
using BNE.BLL;
using BNE.BLL.Enumeradores.CustomAttribute;
using BNE.BLL.Mensagem.Mailsender;
using BNE.ExtensionsMethods;
using log4net;
using MailSender;
using MailSender.Models;

namespace BNE.Services.Candidato
{
    public class NotificarAtualizacaoCurriculoParaPesquisaComPoucosResultados : AbstractJob
    {
        private readonly IMailSenderAPI _mailSenderApi;
        private static readonly string EmailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
        private static readonly string MailSenderAPIKey = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.MailSenderAPIKey);

        public NotificarAtualizacaoCurriculoParaPesquisaComPoucosResultados(ILog logger, IMailSenderAPI mailSenderApi) : base(logger)
        {
            _mailSenderApi = mailSenderApi;
        }

        public override void Execute()
        {
            if (!string.IsNullOrWhiteSpace(MailSenderAPIKey))
            {
                var mailsenderId = BLL.Enumeradores.CartaEmail.AlertaPoucosCV.GetAttribute<MailsenderAttribute>().Id;

                string HoraInicio = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var enviados = new List<Enviados>();
                var pesquisas = PesquisaCurriculo.RecuperarPesquisasComPoucosCurriculos();

                var tempEmailEnviados = new List<string>();

                foreach (var item in pesquisas)
                {
                    try
                    {
                        var curriculos = PesquisaCurriculo.RecuperarCurriculosParaPesquisaComPoucosCurriculos(item);

                        var objEnviado = new Enviados
                        {
                            Area = item.AreaFuncao,
                            Cidade = item.Cidade,
                            Categoria = item.FuncaoCategoria,
                            Estado = item.SiglaEstado
                        };

                        foreach (var curriculo in curriculos)
                        {
                            if (!tempEmailEnviados.Contains(curriculo.Email))
                            {
                                try
                                {
                                    var url = "http://www.bne.com.br/logar/{0}";
#if DEBUG
                                    url = "http://localhost:2000/logar/{0}";
#endif
                                    tempEmailEnviados.Add(curriculo.Email);

                                    if (BLL.Custom.Validacao.ValidarEmail(curriculo.Email))
                                    {
                                        //TODO Fazer o insert em lotes
                                        var logEnvioMensagem = new LogEnvioMensagem
                                        {
                                            EmlDestinatario = curriculo.Email,
                                            CartaEmail = new CartaEmail((int)BLL.Enumeradores.CartaEmail.AlertaPoucosCV),
                                            EmlRemetente = EmailRemetente
                                        };

                                        logEnvioMensagem.Save();

                                        //TODO Fazer o envio em lotes
                                        var to = new List<string> { curriculo.Email };

                                        var parametros = new MailsenderParameters<MailsenderSubstitutionParameters, MailsenderSectionParameters>
                                        {
                                            Substitution = new MailsenderSubstitutionParameters()
                                        };

                                        parametros.Substitution.nome.Add(curriculo.PrimeiroNome);
                                        parametros.Substitution.area.Add(item.AreaFuncao);
                                        parametros.Substitution.URL.Add(String.Format(url, BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(curriculo.CPF, curriculo.DataNascimento, "/cadastro-de-curriculo-gratis")));

                                        _mailSenderApi.Mail.Post(new SendCommand(MailSenderAPIKey, EmailRemetente, to, templateId: mailsenderId, substitution: parametros.Substitution));

                                        objEnviado.Quantidade++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error($"NotificarAtualizarCVPesquisaNumeroInsuficiente enviar alerta pra {curriculo.Email}", ex);
                                }
                            }
                        }
                        enviados.Add(objEnviado);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }

                EnviarResultado(HoraInicio, enviados);
            }
        }

        private void EnviarResultado(string horaInicio, List<Enviados> enviados)
        {
            var TotalEnviados = enviados.Sum(c => c.Quantidade);

            try
            {
                string templateRelatorio = $"<table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"790\"> <tr> <td style=\"line - height: 150 %; \"> <font color=\"#333333\" face=\"Arial, Helvetica, sans-serif\" size=\"5\"> <br> <div>Inicio:{horaInicio} Terminou as:{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}, enviado {TotalEnviados} e-mails, </div> <br> </font> <font color=\"#333333\" face=\"Arial, Helvetica, sans-serif\" size=\"3\"> <div> {{Categoria}} </div> <br> </font> </td> </tr> </table > ";
                var tpCategoria = "<br>Categoria: {Categoria} - {TotalCategoria} <br><ul>{area}</ul> ";
                var linha = "<li>Area: <b>{AreaBNE}</b>  - Cidade - <b>{Cidade}/{Estado}</b> - Envidados - <b>{TotalEnviado}</b></li>";

                #region [Apoio]
                string tpCategoriaReplace = string.Empty, linhaReplace = string.Empty;
                int totalCat = 0;
                foreach (var rel in enviados.FindAll(x => x.Categoria == BLL.Enumeradores.FuncaoCategoria.Apoio.ToString()))
                {
                    linhaReplace += linha.Replace("{AreaBNE}", rel.Area).Replace("{Cidade}", rel.Cidade).Replace("{Estado}", rel.Estado).Replace("{TotalEnviado}", rel.Quantidade.ToString());
                    totalCat++;
                }

                tpCategoriaReplace += tpCategoria.Replace("{Categoria}", BLL.Enumeradores.FuncaoCategoria.Apoio.ToString())
                    .Replace("{TotalCategoria}", totalCat.ToString()).Replace("{area}", linhaReplace);
                #endregion

                #region [Operacao]
                linhaReplace = string.Empty;
                totalCat = 0;
                foreach (var rel in enviados.FindAll(x => x.Categoria == BLL.Enumeradores.FuncaoCategoria.Operacao.ToString()))
                {
                    linhaReplace += linha.Replace("{AreaBNE}", rel.Area).Replace("{Cidade}", rel.Cidade).Replace("{Estado}", rel.Estado).Replace("{TotalEnviado}", rel.Quantidade.ToString());
                    totalCat++;
                }

                tpCategoriaReplace += tpCategoria.Replace("{Categoria}", BLL.Enumeradores.FuncaoCategoria.Operacao.ToString())
                    .Replace("{TotalCategoria}", totalCat.ToString()).Replace("{area}", linhaReplace);
                #endregion

                #region [Especialista]
                linhaReplace = string.Empty;
                totalCat = 0;
                foreach (var rel in enviados.FindAll(x => x.Categoria == BLL.Enumeradores.FuncaoCategoria.Especialista.ToString()))
                {
                    linhaReplace += linha.Replace("{AreaBNE}", rel.Area).Replace("{Cidade}", rel.Cidade).Replace("{Estado}", rel.Estado).Replace("{TotalEnviado}", rel.Quantidade.ToString());
                    totalCat++;
                }

                tpCategoriaReplace += tpCategoria.Replace("{Categoria}", BLL.Enumeradores.FuncaoCategoria.Especialista.ToString())
                    .Replace("{TotalCategoria}", totalCat.ToString()).Replace("{area}", linhaReplace);
                #endregion

                #region [Gestao]
                linhaReplace = string.Empty;
                totalCat = 0;
                foreach (var rel in enviados.FindAll(x => x.Categoria == BLL.Enumeradores.FuncaoCategoria.Gestao.ToString()))
                {
                    linhaReplace += linha.Replace("{AreaBNE}", rel.Area).Replace("{Cidade}", rel.Cidade).Replace("{Estado}", rel.Estado).Replace("{TotalEnviado}", rel.Quantidade.ToString());
                    totalCat++;
                }

                tpCategoriaReplace += tpCategoria.Replace("{Categoria}", BLL.Enumeradores.FuncaoCategoria.Gestao.ToString()).Replace("{TotalCategoria}", totalCat.ToString()).Replace("{area}", linhaReplace);
                #endregion

                templateRelatorio = templateRelatorio.Replace("{Categoria}", tpCategoriaReplace);

                _mailSenderApi.Mail.Post(new SendCommand(MailSenderAPIKey, EmailRemetente, new List<string> { "gieyson@bne.com.br", "mailson@bne.com.br" }, subject: "Relatório: alerta poucos cvs, e-mails enviados", message: templateRelatorio));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private struct Enviados
        {
            public int Quantidade { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Area { get; set; }
            public string Categoria { get; set; }
        }

        public class MailsenderSubstitutionParameters
        {
            public MailsenderSubstitutionParameters()
            {
                nome = new List<string>();
                area = new List<string>();
                URL = new List<string>();
            }
            public IList<string> nome { get; set; }
            public IList<string> area { get; set; }
            public IList<string> URL { get; set; }
        }
        public class MailsenderSectionParameters
        {

        }
    }
}