using System;
using System.Data;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Services.Properties;

namespace BNE.Services
{
    internal partial class EnviarEmailAlertaMensalFilial : BaseService
    {
        #region Construtores
        public EnviarEmailAlertaMensalFilial()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnviarEmailAlertaMensalFilialHoraExecucao;
        private static readonly int DiaExecucao = Settings.Default.EnviarEmailAlertaMensalFilialDiaExecucao;
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnviarEmailAlertaMensalFilial);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Metodos

        #region IniciarEnviarEmailAlertaMensalFilial
        public void IniciarEnviarEmailAlertaMensalFilial()
        {
            try
            {
                //MailController.Send("charan@bne.com.br", "atendimento@bne.com.br", "Teste de e-mail bne", "email teste Charan", SaidaSMTP.SendGrid);
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
#if Release
                    EventLogWriter.LogEvent(String.Format("Iniciou em {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                    EnviarAlertaMensalFilial();

#if Release
                    EventLogWriter.LogEvent(String.Format("Terminou em {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
                    AjustarThread(DateTime.Now, false);
#endif
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
#if Release
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
#endif
            }
        }
        #endregion

        #region EnviarAlertaMensalFilial
        public void EnviarAlertaMensalFilial()
        {
            DataTable dtDadosFilial, dtCidadesGrupo, dtDadosRelatorio = null;

            var mes = DateTime.Now.Month;
            var ano = DateTime.Now.Year;

            var dataInicio = DateTime.Parse(ano + "-" + (mes - 1) + "-01");
            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);

            #region HtmlGrafico

            //StringBuilder sbHtmlGrafico = new StringBuilder();

            //sbHtmlGrafico.Append("https://chart.googleapis.com/chart?cht=bvs&chd=t:60,40&chs=900x500&chl=Hello|Fabiano");
            //sbHtmlGrafico.Append("</html><html><head><script type=\"text/javascript\" src=\"https://www.google.com/jsapi\"></script>");
            //sbHtmlGrafico.Append("<script type=\"text/javascript\">google.load(\"visualization\", \"1\", {packages:[\"corechart\"]});google.setOnLoadCallback(drawChart);function drawChart() {var data = google.visualization.arrayToDataTable([['Currículos', 'Cadastrados', 'Atualizados'],['Agosto',21,32],['Setembro',29,43],['Outubro',350,700]]);var options = {title: 'Currículos Cadastrados',vAxis: {title: 'Mês',  titleTextStyle: {color: 'red'}}};var chart = new google.visualization.BarChart(document.getElementById('chart_div'));chart.draw(data, options);}</script></head><body><div id=\"chart_div\" style=\"width: 900px; height: 500px;\"></div></body></html>");
            #endregion

            try
            {
                //Carregar os grupos e destinatários
                dtDadosFilial = Filial.CarregarGrupoDestinatarioFiliaisEmployer();

                var objcarta =
                    CartaEmail.LoadObject(Convert.ToInt32(BLL.Enumeradores.CartaEmail.EnviarEmailAlertaMensalFilial));
                var carta = CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.EnviarEmailAlertaMensalFilial);

                foreach (DataRow dr in dtDadosFilial.Rows)
                {
                    //Carregar as cidades do grupo
                    dtCidadesGrupo =
                        Filial.CarregarCidadesGrupoFiliaisEmployer(int.Parse(dr["Idf_Grupo_Cidade"].ToString()));
                    var sbIdfCidades = new StringBuilder();
                    //StringBuilder sbNomeCidades = new StringBuilder();

                    foreach (DataRow linha in dtCidadesGrupo.Rows)
                    {
                        sbIdfCidades.AppendFormat("{0},", linha["Idf_Cidade"]);
                        //sbNomeCidades.AppendFormat("{0},", linha["Nme_Cidade"]);
                    }

                    dtDadosRelatorio = Filial.CarregarDadosRelatorioMensalFilialEmployer(dataInicio,
                        dataInicio.AddMonths(1), sbIdfCidades.ToString().Remove(sbIdfCidades.Length - 1));

                    foreach (DataRow resumo in dtDadosRelatorio.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["Des_Email"].ToString()))
                        {
                            carta = carta.Replace("{Mes_Ano}",
                                string.Format("{0} de {1}", Helper.RetornarMesExtenso(mes - 1), ano));
                            carta = carta.Replace("{Total_Cvs_Cadastrados}",
                                resumo["TotalCurriculosCadastrados"].ToString());
                            carta = carta.Replace("{Total_Cvs_Atualizados}",
                                resumo["TotalCurriculosAtualizados"].ToString());
                            carta = carta.Replace("{Total_Vips_Novos}", resumo["VIPNovosMes"].ToString());
                            carta = carta.Replace("{Total_Vips_Ativos}", resumo["VipAtivos"].ToString());
                            carta = carta.Replace("{Total_Vagas_Cadastradas}", resumo["TotalNovasVagas"].ToString());
                            carta = carta.Replace("{Total_Empresas_Cadastradas}",
                                resumo["TotalEmpresasCadastradas"].ToString());
                            carta = carta.Replace("{Total_Empresas_Cadastradas}",
                                resumo["TotalEmpresasPlanoNovo"].ToString());
                            carta = carta.Replace("{Total_Empresas_Com_Plano}",
                                resumo["TotalEmpresasPlanosAtivos"].ToString());

                            //carta = carta.Replace("{CidadesdaRegiao}", sbNomeCidades.ToString());

                            //carta += string.Format("<img src=\"{0}\" />", sbHtmlGrafico.ToString());

                            //para testar o envio utilize a linha abaixo com o seu e-mail.
                            //MailController.Send("charan@bne.com.br", "atendimento@bne.com.br", "Teste de e-mail bne", carta, SaidaSMTP.SendGrid);

                            //enviar email para destinatario responsavel pelo grupo
                            //EmailSenderFactory
                            //    .Create(TipoEnviadorEmail.Fila)
                            //    .Enviar("BNE - Relatório mensal da sua região", carta, emailRemetente, dr["Des_Email"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message;
                GerenciadorException.GravarExcecao(ex, out message);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                var tempoParaExecutar =
                    DateTime.Parse(string.Format("{0}-{1}-{2} {3}", horaAtual.Year, horaAtual.Month + 1, DiaExecucao,
                        HoraExecucao)) - horaAtual;

#if Release
                EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
#endif
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DiaExecucao, 0);
                if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
                {
#if Release
                    EventLogWriter.LogEvent(String.Format("Alerta mensal filial - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
#endif
                }
                else
                {
#if Release
                    EventLogWriter.LogEvent(String.Format("Alerta mensal filial - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, Event.WarningAjusteExecucao);
#endif
                }
            }
        }
        #endregion

        #endregion
    }
}