using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Collections.Generic;
using System.Data.SqlClient;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;

namespace BNE.Services
{
    partial class EnviaEmailEtapasCandidatura : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        #region [Query]

        private const string spEtapaProcessamento = @"select candlog.Idf_vaga_candidato, candlog.Idf_Curriculo, candlog.Idf_Vaga, candlog.Idf_Log_Envio_Candidatura
                                        from BNE.BNE_Log_Envio_Candidatura candlog with(nolock)
                                    where candlog.dta_processamento_candidatura_envio is null
                                    and candlog.dta_processamento_candidatura between @Dta_inicio and @Dta_Fim 

                                        ";

        private const string spEtapaAnalise = @"select candlog.Idf_vaga_candidato,  candlog.Idf_Curriculo, candlog.Idf_Vaga, candlog.Idf_Log_Envio_Candidatura
                                        from BNE.BNE_Log_Envio_Candidatura candlog with(nolock)
	                                   where candlog.dta_analise_cv_envio is null
                                        and candlog.dta_analise_cv between @Dta_inicio and @Dta_Fim ";


        private const string spEtapaEnvio = @"selectcandlog.Idf_vaga_candidato,  candlog.Idf_Curriculo, candlog.Idf_Vaga, candlog.Idf_Log_Envio_Candidatura
                        from BNE.BNE_Log_Envio_Candidatura candlog with(nolock)
	                    where candlog.dta_envio_cv_envio is null
                               and candlog.dta_envio_cv between @Dta_inicio and @Dta_Fim 
                                        ";
        #endregion


        public EnviaEmailEtapasCandidatura()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
      
            GravaLogText("Iniciou o EnviaEmailEtapasCandidatura em " + DateTime.Now);
            //Enviar os e-mails.
            var emailRemetente = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            var emailDestino = string.Empty;
            //Etapa 1 - da candidatura é enviado na hora da candidatura.

            #region [Processamento_Candidatura]
            int countProcesso = 0;
            List<SqlParameter> parametros = new List<SqlParameter>(){
                new SqlParameter{ParameterName="@Dta_Inicio", SqlDbType = System.Data.SqlDbType.DateTime, Value = DateTime.Now.AddMinutes(-6) },
                new SqlParameter{ParameterName = "@Dta_Fim", SqlDbType = System.Data.SqlDbType.DateTime, Value = DateTime.Now }
            };
            var objCarta = BLL.CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.CandEtapaAnalise);
            using (var dr = DataAccessLayer.ExecuteReader(System.Data.CommandType.Text, spEtapaProcessamento, parametros))
            {
                while (dr.Read())
                {
                    var assunto = objCarta.Assunto;
                    //Enviar e-mail
                    var layoutCarta =  LogEnvioCandidatura.MontarEmail(objCarta.Conteudo, ref assunto, Convert.ToInt32(dr["idf_vaga_candidato"]), out emailDestino);
                    //layoutCarta.Replace
                    if (!string.IsNullOrEmpty(emailDestino))
                    {
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                     .Enviar(assunto, layoutCarta, BLL.Enumeradores.CartaEmail.CandEtapaAnalise, emailRemetente, emailDestino);

                        LogEnvioCandidatura.AtualizarEnvioEmail(BLL.Enumeradores.CartaEmail.CandEtapaProcessamento, Convert.ToInt32(dr["Idf_Log_Envio_Candidatura"]));
                        countProcesso++;
                    }
                }


            }


            GravaLogText($"Enviados {countProcesso} etapa processamento. ");
            #endregion

            #region [Analise]

            int countEntregue = 0;
            parametros = new List<SqlParameter>(){
                new SqlParameter{ParameterName = "@Dta_Inicio", SqlDbType = System.Data.SqlDbType.DateTime, Value = DateTime.Now.AddMinutes(-6) },
                new SqlParameter{ParameterName = "@Dta_Fim", SqlDbType = System.Data.SqlDbType.DateTime, Value = DateTime.Now }
            };
            objCarta = BLL.CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.CandEtapaEnvio);
            using (var dr = DataAccessLayer.ExecuteReader(System.Data.CommandType.Text, spEtapaAnalise, parametros))
            {
                while (dr.Read())
                {
                    var assunto = objCarta.Assunto;
                    var layoutCarta = LogEnvioCandidatura.MontarEmail(objCarta.Conteudo, ref assunto, Convert.ToInt32(dr["idf_vaga_candidato"]), out emailDestino);
                    //layoutCarta.Replace
                    if (!string.IsNullOrEmpty(emailDestino))
                    {
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                     .Enviar(objCarta.Assunto, layoutCarta, BLL.Enumeradores.CartaEmail.CandEtapaEnvio, emailRemetente, emailDestino);
                     
                        LogEnvioCandidatura.AtualizarEnvioEmail(BLL.Enumeradores.CartaEmail.CandEtapaAnalise, Convert.ToInt32(dr["Idf_Log_Envio_Candidatura"]));
                        countEntregue++;
                    }
                }
            }

            GravaLogText($"Enviados {countEntregue} etapa curriculo entregue. ");

            #endregion

            //Etapa 4 - do curriculo visualizado é enviado no processo da visualização

        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo EnviaEmailEtapasCandidatura.");

            _scheduler.Start();
            var job = JobBuilder.Create<EnviaEmailEtapasCandidatura>()
                .Build();

            ITrigger trigger;

            trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(5)
                        .OnEveryDay()
                  )
                .Build();

            _scheduler.ScheduleJob(job, trigger);



        }

        protected override void OnStop()
        {
            GravaLogText("EnviaEmailEtapasCandidatura desativado.");
            _scheduler.Shutdown();
        }

        private static void GravaLogText(string mensagem)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "logEtapasCandidatura.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
            }
        }


    }
}
