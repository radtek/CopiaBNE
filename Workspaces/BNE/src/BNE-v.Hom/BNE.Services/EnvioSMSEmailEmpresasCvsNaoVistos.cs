using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using CartaEmail = BNE.BLL.CartaEmail;
using FormatObject = BNE.BLL.Common.FormatObject;
using Vaga = BNE.BLL.Vaga;
using BNE.BLL.Mensagem.DTO;

namespace BNE.Services
{
    public partial class EnvioSMSEmailEmpresasCvsNaoVistos : BaseService
    {
        #region Construtores
        public EnvioSMSEmailEmpresasCvsNaoVistos(IContainer container) : this()
        {
            container.Add(this);
        }

        public EnvioSMSEmailEmpresasCvsNaoVistos()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(Iniciar);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        #region Iniciar
        public void Iniciar()
        {
            try
            {
                Settings.Default.Reload();
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    try
                    {
                        EventLogWriter.LogEvent(
                            string.Format("Robo EnvioSMSEmailEmpresasCvsNaoVistos - Iniciado: {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);
                        _dataHoraUltimaExecucao = DateTime.Now;

                        InicializarEnvioSMSEmailEmpresasCvsNaoVistos();

                        //Grava os Logs de sucesso
                        EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.FimExecucao);
                    }
                    catch (Exception ex)
                    {
                        GravaLogErro(ex, "Iniciar() - robo EnvioSMSEmailEmpresasCvsNaoVistos");
                    }

                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex) //PAROU O ROBO
            {
                GerenciadorException.GravarExcecao(ex, "appBNE - Erro no robo: EnvioSMSEmailEmpresasCvsNaoVistos");
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar("appBNE - Erro: EnvioSMSEmailEmpresasCvsNaoVistos",
                        "appBNE - Erro no robo: EnvioSMSEmailEmpresasCvsNaoVistos", null, "martysroka@bne.com.br",
                        "martysroka@bne.com.br");

                GravaLogErro(ex, "Iniciar() - Parou a execução do robo EnvioSMSEmailEmpresasCvsNaoVistos");
            }
        }
        #endregion

        private DateTimeOffset GetNextExecution()
        {
            var now = DateTime.Now;
            var horaMinuto = Settings.Default.EnvioSMSEmailEmpresasCvsNaoVistosHoraExecucao.Split(':');

            var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

            if (horaParaExecucao.Subtract(now).Ticks < 0)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            if (horaParaExecucao.DayOfWeek == DayOfWeek.Saturday)
                horaParaExecucao = horaParaExecucao.AddDays(2);

            if (horaParaExecucao.DayOfWeek == DayOfWeek.Sunday)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            return horaParaExecucao;
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

        #region Métodos

        #region InicializarEnvioSMSEmailEmpresasCvsNaoVistos
        /// <summary>
        ///     Serviço executado nos dias de semana para envio de SMS e E-Mail a usuários de empresas.
        ///     Verifica quais usuários cadastraram vagas a 5 dias úteis atrás E tem Cvs não lidos.
        ///     *Seleciona apenas as 5 últimas vagas cadastradas (a 5 dias úteis atrás) de cada usuário,
        ///     se tem mais de 5 vagas com Cvs não lidos, o usuário será notificado apenas das 5 ultimas vagas.
        /// </summary>
        /// <returns></returns>
        public void InicializarEnvioSMSEmailEmpresasCvsNaoVistos()
        {
            var listaSMS = new List<DestinatarioSMS>();
            var IdfUfpEnvioSMS =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueAvisoCvsNaoVistos);
            var objSMSDestinatario = new DestinatarioSMS();
            var cartaSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.SMSAlerta_CvsNaoVistos);

            //Separa dataInicial e dataFinal para consulta das procs
            var dataInicial = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            if (DateTime.Now.DayOfWeek != DayOfWeek.Monday)
                dataInicial = dataInicial.AddDays(-7); //volta 7 dias para pegar cadastros de 5 dias úteis atras
            else
                dataInicial = dataInicial.AddDays(-9);
            //se for segunda, volta 9 dias para incluir sábado, domingo e segunda na consulta

            var dataFinal = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            dataFinal = dataFinal.AddDays(-7); //volta 7 dias para pegar cadastros de 5 dias úteis atras

            var dtUsuarios = Vaga.ListaUsuariosCadastraramVagas(dataInicial, dataFinal);

            foreach (DataRow drUsuario in dtUsuarios.Rows)
            {
                var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
                if (!string.IsNullOrEmpty(drUsuario["EmailVendedorResponsavel"].ToString()))
                    emailRemetente = drUsuario["EmailVendedorResponsavel"].ToString();

                //Verifica das Vagas cadastradas, se tem CvsNaoVistos e a qual a quantidade 'qtdeCvsNaoVistos'
                var dtVagasUsuario =
                    Vaga.ListaUltimasVagasCadastradasPorUsuario(
                        Convert.ToInt32(drUsuario["Idf_Usuario_Filial_Perfil"]), dataInicial, dataFinal);
                foreach (DataRow drVaga in dtVagasUsuario.Rows)
                {
                    var primeiroNome = PessoaFisica.RetornarPrimeiroNome(drUsuario["Nme_Pessoa"].ToString());

                    var objVaga = Vaga.LoadObject(Convert.ToInt32(drVaga["Idf_Vaga"]));
                    objVaga.Funcao.CompleteObject();

                    var textoBotaoVerCurriculo = "Ver currículo";
                    var curriculo = "candidato inscrito não visualizado";
                    if (Convert.ToInt32(drVaga["qtdeCvsNaoVistos"]) > 1)
                    {
                        textoBotaoVerCurriculo = "Ver currículos";
                        curriculo = "candidatos inscritos não visualizados";
                    }

                    #region SMS
                    if (!string.IsNullOrEmpty(drUsuario["Num_DDD_Celular"].ToString()) &&
                        !string.IsNullOrEmpty(drUsuario["Num_Celular"].ToString()))
                    {
                        objSMSDestinatario = new DestinatarioSMS
                        {
                            IdDestinatario = Convert.ToInt32(drUsuario["Idf_Usuario_Filial_Perfil"]),
                            NomePessoa = Convert.ToString(drUsuario["Nme_Pessoa"]),
                            DDDCelular = Convert.ToString(drUsuario["Num_DDD_Celular"]),
                            NumeroCelular = Convert.ToString(drUsuario["Num_Celular"])
                        };
                        objSMSDestinatario.Mensagem = string.Format(cartaSMS, drVaga["qtdeCvsNaoVistos"], curriculo,
                            objVaga.Funcao.DescricaoFuncao);
                        listaSMS.Add(objSMSDestinatario);
                    }
                    #endregion SMS

                    #region E-Mail
                    if (!string.IsNullOrEmpty(drUsuario["Eml_Comercial"].ToString()))
                    {
                        var objcarta =
                            CartaEmail.LoadObject(
                                Convert.ToInt32(BLL.Enumeradores.CartaEmail.ConteudoEmailAvisoCvsNaoVistos));
                        var assunto = objcarta.DescricaoAssunto;

                        var _estatisticas = Estatistica.Estatisticas;
                        var urlAmbiente = Helper.RecuperarURLAmbiente();
                        var linkPesquisaCurriculosNaoVistosDaVaga = string.Empty;
                        var linkUrlHomeBNE = string.Empty;
                        var linkPaginaUsuario = string.Empty;
                        var linkAnunciarVagas = string.Empty;
                        var linkSalaSelecionadora = string.Empty;
                        var linkCompreCurriculos = string.Empty;
                        var linkAtualizarEmpresa = string.Empty;
                        var linkCvsRecebidos = string.Empty;

                        UsuarioFilialPerfil.RetornarHashLogarUsuarioEmpresa_CvsNaoVistosVaga(
                            Convert.ToInt32(drUsuario["Idf_Pessoa_Fisica"]), Convert.ToInt32(drVaga["Idf_Vaga"]),
                            out linkPesquisaCurriculosNaoVistosDaVaga,
                            out linkUrlHomeBNE,
                            out linkPaginaUsuario,
                            out linkAnunciarVagas,
                            out linkSalaSelecionadora,
                            out linkCompreCurriculos,
                            out linkAtualizarEmpresa,
                            out linkCvsRecebidos);
                        var parametrosCorpoEmail = new
                        {
                            NomePessoa = primeiroNome,
                            curriculoCorpo = curriculo,
                            NomeFuncao = objVaga.Funcao.DescricaoFuncao + " (" + objVaga.CodigoVaga + ")",
                            QtdCvsNaoVistos = drVaga["qtdeCvsNaoVistos"].ToString(),
                            linkPesquisaCurriculosNaoVistosDaVaga,
                            textoLinkPesquisaCurriculosNaoVistosDaVaga = textoBotaoVerCurriculo,
                            linkDesativarVaga = linkCvsRecebidos,
                            Quantidade_curriculos = _estatisticas.QuantidadeCurriculo.ToString(),
                            UrlBNE = linkUrlHomeBNE,
                            UrlPaginaUsuario = linkPaginaUsuario,
                            UrlAnunciarVaga = linkAnunciarVagas,
                            UrlSalaSelecionador = linkSalaSelecionadora,
                            UrlComprarCurriculos = linkCompreCurriculos,
                            UrlAtualizarEmpresa = linkAtualizarEmpresa,
                            UrlCvsRecebidos = linkCvsRecebidos
                        };
                        var mensagem = FormatObject.ToString(parametrosCorpoEmail, objcarta.ValorCartaEmail);


                        var primeira = char.ToUpper(curriculo[0]);
                        curriculo = primeira + curriculo.Substring(1);
                        assunto =
                            objcarta.DescricaoAssunto =
                                objcarta.DescricaoAssunto.Replace("{curriculoAssunto}", curriculo)
                                    .Replace("{NomeFuncao}", objVaga.Funcao.DescricaoFuncao);
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                            .Enviar(assunto, mensagem,BLL.Enumeradores.CartaEmail.ConteudoEmailAvisoCvsNaoVistos, emailRemetente, drUsuario["Eml_Comercial"].ToString());
                    }
                    #endregion E-Mail
                }
            }
            if (listaSMS.Count > 0)
                MensagemCS.EnvioSMSTanque(IdfUfpEnvioSMS, listaSMS, true);
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            Settings.Default.Reload();
            var DelayExecucao = Settings.Default.EnvioSMSEmailEmpresasCvsNaoVistosDelayMinutos;
            var HoraExecucao = Settings.Default.EnvioSMSEmailEmpresasCvsNaoVistosHoraExecucao;

            if (primeiraExecucao)
            {
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format("O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
                    EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int) tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region GravaLogErro
        protected void GravaLogErro(Exception ex, string metodo)
        {
            try
            {
                var message =
                    string.Format("appBNE - Erro no Robo EnvioSMSEmailEmpresasCvsNaoVistos. Hora: {0}. Erro: {1}",
                        DateTime.Now.ToLongTimeString(), ex);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);

                var sCustomMessage = "appBNE - Erro no Robo EnvioSMSEmailEmpresasCvsNaoVistos. Método: " + metodo;
                GerenciadorException.GravarExcecao(ex, sCustomMessage);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #endregion
    }
}