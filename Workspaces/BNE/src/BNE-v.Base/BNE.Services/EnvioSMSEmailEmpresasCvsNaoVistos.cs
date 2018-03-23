using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BNE.Services
{
    public partial class EnvioSMSEmailEmpresasCvsNaoVistos : ServiceBase
    {
        public EnvioSMSEmailEmpresasCvsNaoVistos(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region Propriedades
        private static readonly string HoraExecucao = Settings.Default.EnvioSMSEmailEmpresasCvsNaoVistosHoraExecucao;
        private const string EventSourceName = "EnvioSMSEmailEmpresasCvsNaoVistos";
        private static DateTime _dataHoraUltimaExecucao;
        private IDisposable _subscription;
        #endregion

        #region Construtores
        public EnvioSMSEmailEmpresasCvsNaoVistos()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), (next) =>
            {
                try
                {
                    InicializarEnvioSMSEmailEmpresasCvsNaoVistos().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(EventSourceName, String.Format("EnvioSMSEmailEmpresasCvsNaoVistos Finalizado: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(EventSourceName, String.Format("EnvioSMSEmailEmpresasCvsNaoVistos ERRO ao completar ({0}): {1}.", t.Status, DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        if (t.Status == TaskStatus.Faulted)
                        {
                            var exp = t.Exception;
                            var details = string.Empty;
                            EL.GerenciadorException.GravarExcecao(exp, details + " (EnvioSMSEmailEmpresasCvsNaoVistos)");
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("appBNE - Erro: EnvioSMSEmailEmpresasCvsNaoVistos (" + details + ")", exp.DumpExInternal(), "valerianeves@bne.com.br", "valerianeves@bne.com.br");
                        }

                    });

                }
                finally
                {
                    next(GetNextExecution());
                }
            });
        }

        private DateTimeOffset GetNextExecution()
        {
            var now = DateTime.Now;
            string[] horaMinuto = Settings.Default.EnvioSMSEmailEmpresasCvsNaoVistosHoraExecucao.Split(':');

            DateTime horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

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
            if (_subscription != null)
                _subscription.Dispose();
        }
        #endregion

        #endregion

        #region Métodos

        #region InicializarEnvioSMSEmailEmpresasCvsNaoVistos
        /// <summary>
        /// Serviço executado nos dias de semana para envio de SMS e E-Mail a usuários de empresas. 
        /// Verifica quais usuários cadastraram vagas a 5 dias úteis atrás E tem Cvs não lidos.
        /// *Seleciona apenas as 5 últimas vagas cadastradas (a 5 dias úteis atrás) de cada usuário, 
        /// se tem mais de 5 vagas com Cvs não lidos, o usuário será notificado apenas das 5 ultimas vagas.
        /// </summary>
        /// <returns></returns>
        public Task InicializarEnvioSMSEmailEmpresasCvsNaoVistos()
        {
            return Task.Factory.StartNew(() =>
            {
                List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();
                var IdfUfpEnvioSMS = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueAvisoCvsNaoVistos);
                var objSMSDestinatario = new BLL.DTO.PessoaFisicaEnvioSMSTanque();
                var cartaSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.SMSAlerta_CvsNaoVistos);

                //Separa dataInicial e dataFinal para consulta das procs
                DateTime dataInicial = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
                if (DateTime.Now.DayOfWeek != DayOfWeek.Monday)
                    dataInicial = dataInicial.AddDays(-7); //volta 7 dias para pegar cadastros de 5 dias úteis atras
                else
                    dataInicial = dataInicial.AddDays(-9); //se for segunda, volta 9 dias para incluir sábado, domingo e segunda na consulta

                DateTime dataFinal = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
                dataFinal = dataFinal.AddDays(-7); //volta 7 dias para pegar cadastros de 5 dias úteis atras

                DataTable dtUsuarios = Vaga.ListaUsuariosCadastraramVagas(dataInicial, dataFinal);

                foreach (DataRow drUsuario in dtUsuarios.Rows)
                {
                    string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
                    if (!string.IsNullOrEmpty(drUsuario["EmailVendedorResponsavel"].ToString()))
                        emailRemetente = drUsuario["EmailVendedorResponsavel"].ToString();

                    //Verifica das Vagas cadastradas, se tem CvsNaoVistos e a qual a quantidade 'qtdeCvsNaoVistos'
                    DataTable dtVagasUsuario = Vaga.ListaUltimasVagasCadastradasPorUsuario(Convert.ToInt32(drUsuario["Idf_Usuario_Filial_Perfil"]), dataInicial, dataFinal);
                    foreach (DataRow drVaga in dtVagasUsuario.Rows)
                    {
                        var primeiroNome = PessoaFisica.RetornarPrimeiroNome(drUsuario["Nme_Pessoa"].ToString());

                        var objVaga = Vaga.LoadObject(Convert.ToInt32(drVaga["Idf_Vaga"]));
                        objVaga.Funcao.CompleteObject();

                        string textoBotaoVerCurriculo = "Ver currículo";
                        string curriculo = "candidato inscrito não visualizado";
                        if (Convert.ToInt32(drVaga["qtdeCvsNaoVistos"]) > 1)
                        {
                            textoBotaoVerCurriculo = "Ver currículos";
                            curriculo = "candidatos inscritos não visualizados";
                        }

                        #region SMS
                        if (!string.IsNullOrEmpty(drUsuario["Num_DDD_Celular"].ToString()) && !string.IsNullOrEmpty(drUsuario["Num_Celular"].ToString()))
                        {
                            objSMSDestinatario = new BLL.DTO.PessoaFisicaEnvioSMSTanque
                            {
                                idDestinatario = Convert.ToInt32(drUsuario["Idf_Usuario_Filial_Perfil"]),
                                nomePessoa = Convert.ToString(drUsuario["Nme_Pessoa"]),
                                dddCelular = Convert.ToString(drUsuario["Num_DDD_Celular"]),
                                numeroCelular = Convert.ToString(drUsuario["Num_Celular"])
                            };
                            objSMSDestinatario.mensagem = string.Format(cartaSMS, drVaga["qtdeCvsNaoVistos"].ToString(), curriculo, objVaga.Funcao.DescricaoFuncao.ToString());
                            listaSMS.Add(objSMSDestinatario);
                        }
                        #endregion SMS

                        #region E-Mail
                        if (!string.IsNullOrEmpty(drUsuario["Eml_Comercial"].ToString()))
                        {
                            CartaEmail objcarta = CartaEmail.LoadObject(Convert.ToInt32(BNE.BLL.Enumeradores.CartaEmail.ConteudoEmailAvisoCvsNaoVistos));
                            string assunto = objcarta.DescricaoAssunto;

                            Estatistica _estatisticas = Estatistica.RecuperarEstatistica();
                            var urlAmbiente = BLL.Custom.Helper.RecuperarURLAmbiente();
                            string linkPesquisaCurriculosNaoVistosDaVaga;
                            string linkUrlHomeBNE;
                            string linkPaginaUsuario;
                            string linkAnunciarVagas;
                            string linkSalaSelecionadora;
                            string linkCompreCurriculos;
                            string linkAtualizarEmpresa;
                            string linkCvsRecebidos;

                            BNE.BLL.UsuarioFilialPerfil.RetornarHashLogarUsuarioEmpresa_CvsNaoVistosVaga(Convert.ToInt32(drUsuario["Idf_Pessoa_Fisica"]), Convert.ToInt32(drVaga["Idf_Vaga"]),
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
                                NomeFuncao = objVaga.Funcao.DescricaoFuncao.ToString() + " (" + objVaga.CodigoVaga + ")",
                                QtdCvsNaoVistos = drVaga["qtdeCvsNaoVistos"].ToString(),
                                linkPesquisaCurriculosNaoVistosDaVaga = linkPesquisaCurriculosNaoVistosDaVaga,
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
                            string mensagem = parametrosCorpoEmail.ToString(objcarta.ValorCartaEmail);


                            char primeira = char.ToUpper(curriculo[0]);
                            curriculo = primeira + curriculo.Substring(1);
                            assunto = objcarta.DescricaoAssunto = objcarta.DescricaoAssunto.Replace("{curriculoAssunto}", curriculo).Replace("{NomeFuncao}", objVaga.Funcao.DescricaoFuncao.ToString());
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, emailRemetente, drUsuario["Eml_Comercial"].ToString());
                        }
                        #endregion E-Mail
                    }
                }
                if (listaSMS.Count > 0)
                    Mensagem.EnvioSMSTanque(IdfUfpEnvioSMS, listaSMS, true);
            });
        }

        #endregion

        #endregion

    }
}