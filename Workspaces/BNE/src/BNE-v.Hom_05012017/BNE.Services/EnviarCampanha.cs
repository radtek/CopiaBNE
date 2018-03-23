using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace BNE.Services
{
    partial class EnviarCampanha : BaseService
    {
        #region Construtores
        public EnviarCampanha()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades

        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnviarCampanhaHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnviarCampanhaDelay;
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
               // #if !DEBUG
                    AjustarThread(DateTime.Now, true);
               // #endif
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    try
                    {
                      
                        #if !DEBUG
                            EventLogWriter.LogEvent(String.Format("Iniciou agora o envio da Campanha do dia {0}.", DateTime.Now), 
                            EventLogEntryType.Information, Event.InicioExecucao);
                        #endif
                        List<CampanhaMarketing> listEnviar = new List<CampanhaMarketing>();
                        //Pegar os selecionados para receber a campanha.
                        string spCampanha = "SELECT top 7000 Num_CPF,Dta_Nascimento,Eml_Pessoa, idf_Curriculo FROM bne.TAB_Campanha_Hotmail ";
                        using (var Campanha = DataAccessLayer.ExecuteReaderDs(CommandType.Text, spCampanha, null).Tables[0])
                        {
                            foreach (DataRow camp in Campanha.Rows)
                            {
                                try
                                {
                                    CampanhaMarketing objCamp = new CampanhaMarketing();
                                    objCamp.idCurriculo = Convert.ToInt32(camp["Idf_Curriculo"]);
                                    objCamp.cpf = Convert.ToDecimal(camp["num_cpf"]);
                                    objCamp.dtaNascimento = Convert.ToDateTime(camp["dta_nascimento"]);
                                    objCamp.email = camp["eml_pessoa"].ToString();
                                    listEnviar.Add(objCamp);
                                }
                                catch (Exception ex)
                                {
                                  
                                }
                            }
                        }
                        
                        if (!listEnviar.IsEmpty())
                        {
                            EnviaEmailCampanha(listEnviar);
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
                   
                    EventLogWriter.LogEvent(String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
                    AjustarThread(DateTime.Now, false);
               
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

        public static void EnviaEmailCampanha(List<CampanhaMarketing> dtCampanha)
        {
            var count = 0;
            var emailEnvalidos = 0;
            AmazonEmailService emailService = new AmazonEmailService();
            //pegar carta para o Email
                var objcartaCampanha =
                        CartaEmail.LoadObject(Convert.ToInt32(BLL.Enumeradores.CartaEmail.CampanhaMarketing));
                var emailRemetente =
                        Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);

                //Parallel.ForEach(dtCampanha, Campanha =>
            foreach(var campanha in dtCampanha)
                {
                    if (Validacao.ValidarEmail(campanha.email))
                    {
                        var carta = objcartaCampanha.ValorCartaEmail;
                        var assunto = objcartaCampanha.DescricaoAssunto;

                        #region CorpoEmail
                        var url = "http://www.bne.com.br/logar/{0}";
                        #if DEBUG
                           url = "http://localhost:2000/logar/{0}";
                        #endif
                       
                        carta = carta.Replace("{AcessoConta}",
                            string.Format(url,
                                LoginAutomatico.GerarHashAcessoLogin(campanha.cpf,
                                    campanha.dtaNascimento,
                                    "/vip")));

                        #endregion

                        //Enviar E-mail para o candidato
                        if(emailService.EnviarEmail(campanha.email, emailRemetente, assunto, carta)){
                             CampanhaHotmail.AtualizarEnvio(campanha.idCurriculo);
                        }
                        count++;
                        //Thread.Sleep(11);
                    }//fim if do verifica email
                    else
                     emailEnvalidos ++;

                    

                }

                //emailService.EnviarEmail("mailsongc@gmail.com", emailRemetente, "Campanha",
               // string.Format("Email enviados: {0}, email invalidos: {1}, encerrado as {2}", count, emailEnvalidos, DateTime.Now));
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
            AmazonEmailService emailService = new AmazonEmailService();
          
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);
                if (!primeiraExecucao)
                    horaParaExecucao = horaParaExecucao.AddDays(1);
                while (!horaParaExecucao.DayOfWeek.Equals(DayOfWeek.Monday) )
                {
                    if (horaParaExecucao.DayOfWeek.Equals(DayOfWeek.Wednesday))
                        break;
                    horaParaExecucao = horaParaExecucao.AddDays(1);
                }
                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                emailService.EnviarEmail("mailsongc@gmail.com", "atendimento@bne.com.br", "Dia de disparar Campanha", String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()));
                EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
           

        }
        #endregion

        #endregion
    }
   
}

public class CampanhaMarketing
{
    public int idCurriculo { get; set; }
    public decimal cpf { get; set; }
    public DateTime dtaNascimento { get; set; }
    public string email { get; set; }
}

