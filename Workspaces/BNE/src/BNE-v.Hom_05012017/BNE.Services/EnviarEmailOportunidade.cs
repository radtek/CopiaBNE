using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using CartaEmail = BNE.BLL.CartaEmail;
using Parametro = BNE.BLL.Parametro;

namespace BNE.Services
{
    internal partial class EnviarEmailOportunidade : BaseService
    {
        #region Construtores
        public EnviarEmailOportunidade()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnviarEmailOportunidadeHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnviarEmailOportunidadeDelay;
        private static readonly string rotaCurriculo = Rota.RecuperarURLRota(RouteCollection.CurriculosPorFuncaoCidade);
        private DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvio);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #region Métodos

        #region IniciarEnvio
        public void IniciarEnvio()
        {
            try
            {
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    try
                    {
#if !DEBUG
                            EventLogWriter.LogEvent(String.Format("Iniciou agora o envio da carta para empresa com as oportunidades do dia {0}.", DateTime.Now), 
                            EventLogEntryType.Information, Event.InicioExecucao);
#endif

                        //Pegar vagas oportunidade arquivadas nos ultimos 30 dias
                        var dtVaga = Vaga.VagasOportunidade();

                        if (dtVaga != null || dtVaga.Rows.Count > 0)
                        {
                            EnviaEmailOportunidade(dtVaga);
                        }
                    }
                    catch (Exception exEnvio)
                    {
#if !DEBUG
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(exEnvio, out message);
                        message = string.Format("{0} - {1}", id, message);
                        
                                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                        
#endif
                    }
#if !DEBUG
                      EventLogWriter.LogEvent(String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
                      AjustarThread(DateTime.Now, false);
#endif
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                    EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);     
#endif
            }
        }
        #endregion

        public static void EnviaEmailOportunidade(DataTable dtVaga)
        {
            var count = 0;


            foreach (DataRow row in dtVaga.Rows)
            {
                var results = VagaCandidato.CandidatosOportunidade(Convert.ToInt32(row["idf_Vaga"]),
                    Convert.ToDateTime(row["dta_prazo"]));

                if (results.Rows.Count > 0)
                {
                    //pegar carta para o Email
                    var objcartaOportunidade =
                        CartaEmail.LoadObject(Convert.ToInt32(BLL.Enumeradores.CartaEmail.OportunidadeEmpresa));

                    var emailRemetente =
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                    var carta = "";


                    carta = objcartaOportunidade.ValorCartaEmail;
                    carta = carta.Replace("{Empresa}", row["nme_empresa"].ToString());

                    #region CorpoEmail
                    var url = "http://www.bne.com.br/logar/{0}";
#if DEBUG
                    url = "http://localhost:2000/logar/{0}";
#endif


                    var oFilial = Filial.LoadObject(Convert.ToInt32(row["idf_filial"]));
                    objcartaOportunidade.DescricaoAssunto =
                        objcartaOportunidade.DescricaoAssunto.Replace("{Funcao}", row["des_funcao"].ToString())
                            .Replace("{cidade}", row["nme_cidade"].ToString())
                            .Replace("{sigEstado}", row["Sig_Estado"].ToString());

                    carta = carta.Replace("{cidade}", string.Format("{0}/{1}", row["nme_Cidade"], row["Sig_Estado"]));
                    carta = carta.Replace("{Funcao}", row["des_funcao"].ToString());
                    var acesso = LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(row["num_cpf"]),
                        Convert.ToDateTime(row["dta_nascimento"]),
                        "/sala-selecionador-vagas-anunciadas");

                    carta = carta.Replace("{ReativarVaga}",
                        string.Format(url,
                            acesso +
                            "?utm_source=carta_oportunidade_empresa&utm_medium=email&utm_campaign=reativar_vaga"));

                    carta = carta.Replace("{AcessoConta}",
                        string.Format(url,
                            LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(row["num_cpf"]),
                                Convert.ToDateTime(row["dta_nascimento"]),
                                "/sala-selecionador")) +
                        "?utm_source=carta_oportunidade_empresa&utm_medium=email&utm_campaign=acesso_conta");
                    carta = carta.Replace("{Vendedor}", oFilial.Vendedor().NomeVendedor);

                    acesso = LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(row["num_cpf"]),
                        Convert.ToDateTime(row["dta_nascimento"]), "/" +
                                                                   rotaCurriculo.Replace("{Funcao}",
                                                                       row["des_funcao"].ToString())
                                                                       .Replace("{Cidade}", row["nme_Cidade"].ToString())
                                                                       .Replace("{SiglaEstado}",
                                                                           row["Sig_Estado"].ToString())
                                                                       .Replace(" ", "-")
                                                                       .ToLower());
                    carta = carta.Replace("{PesquisaCurriculo}", string.Format(url, acesso +
                                                                                    "?utm_source=carta_oportunidade_empresa&utm_medium=email&utm_campaign=pesquisa_curriculo"));
                    #endregion

                    #region MontaCandidatos
                    var cartaCandidato =
                        CartaEmail.LoadObject(
                            Convert.ToInt32(BLL.Enumeradores.CartaEmail.OportunidadeEmpresaCorpoCandidato));
                    var sb = new StringBuilder();

                    foreach (DataRow dr in results.Rows)
                    {
                        var candidato = cartaCandidato.ValorCartaEmail;
                        candidato = candidato.Replace("{Candidato}", dr["nme_pessoa"].ToString());
                        candidato = candidato.Replace("{Funcao}", dr["des_funcao"].ToString());
                        candidato = candidato.Replace("{Idade}",
                            Helper.CalcularIdade(Convert.ToDateTime(dr["dta_nascimento"].ToString())).ToString());
                        candidato = candidato.Replace("{Cidade}",
                            string.Format("{0}/{1}", dr["nme_Cidade"], dr["Sig_Estado"]));
                        candidato = candidato.Replace("{VerCurriculo}",
                            string.Format(url,
                                LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(row["num_cpf"]),
                                    Convert.ToDateTime(row["dta_nascimento"]),
                                    SitemapHelper.MontarCaminhoVisualizacaoCurriculo(dr["des_funcao"].ToString(),
                                        dr["nme_cidade"].ToString(), dr["Sig_Estado"].ToString(),
                                        Convert.ToInt32(dr["idf_curriculo"])).ToLower())) +
                            "?idVaga=" + row["idf_vaga"].ToString() + "&utm_source=carta_oportunidade_empresa&utm_medium=email&utm_campaign=cv_" + dr["idf_curriculo"].ToString());
                        sb.Append(candidato);
                    }

                    carta = carta.Replace("{Candidatos}", sb.ToString());
                    #endregion

                    if (Validacao.ValidarEmail(row["eml_comercial"].ToString()))
                    {
                        //Enviar E-mail para o candidato
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                            .Enviar(objcartaOportunidade.DescricaoAssunto, carta,BLL.Enumeradores.CartaEmail.OportunidadeEmpresa, emailRemetente,
                                row["eml_comercial"].ToString());
                        count++;
                    }
                }
            }
            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                .Enviar("Email Oportunidade", string.Format("Email enviados: {0} ", count),null, "mailson@bne.com.br",
                    "mailson@bne.com.br");
        }

        #region TratarSalario
        public static string TratarSalarioVaga(string de, string ate)
        {
            var retorno = "";

            if (de == "" && ate == "")
                return " a combinar";

            if (de != null)
            {
                retorno += string.Format("de: {0}", de);
            }
            if (ate != null)
            {
                retorno += string.Format(" até: {0}", ate);
            }

            return retorno;
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {

            if (primeiraExecucao)
            {
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }

        }
        #endregion

        #endregion
    }
}