using BNE.BLL.Notificacao;
using BNE.EL;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BNE.Services.AvisoInstantaneoDeVagas
{
    partial class AvisoDeVagas : ServiceBase
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private readonly int _intervaloExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloExecucao"].ToString());

        #region Construtor
        public AvisoDeVagas()
        {
            InitializeComponent();
        }
        #endregion

        #region OnStart
        protected override void OnStart(string[] args)
        {
            ConfigureScheduler();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
        #endregion

        #region ConfigureScheduler
        public void ConfigureScheduler()
        {
            _scheduler.Start();
            var job = JobBuilder.Create<AvisoDeVagasJob>().Build();

            var interval = _intervaloExecucao;


            ITrigger trigger;
#if DEBUG
            trigger = TriggerBuilder.Create().StartNow().Build();
#else

            trigger = TriggerBuilder.Create()
             .StartAt(DateTime.Now)
             .WithSimpleSchedule
               (s =>
                  s.WithIntervalInMinutes(interval)
                  .RepeatForever()
               )
             .Build();

#endif
            _scheduler.ScheduleJob(job, trigger);
        }
        #endregion

        #region AvisoDeVagasJob

        [DisallowConcurrentExecution]
        private class AvisoDeVagasJob : IJob
        {
            private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            private readonly AvisoInstantaneoDeVagasVipService _avisoDeVagasService = new AvisoInstantaneoDeVagasVipService();

           public void Execute(IJobExecutionContext context)
           {
                try
                {
    
                    GravaLogText($"Inicio Processamento às -> { DateTime.Now.ToString() }");
                    _logger.Debug("Aviso Instantâneo de Vagas iniciou agora..." + DateTime.Now);

                    var allProcess = _avisoDeVagasService.GetAllToProcess();

                    //Atualiza as datas de inicio dos alertas para evitar duplicidade
                    ProcessamentoAvisoVagasVip.AtualizarDataInicio(allProcess);

                    //foreach (var avisovagas in allProcess)
                    Parallel.ForEach(allProcess, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism() }, avisovagas =>
                    {
                        try
                        {
                            _logger.Debug($"Aviso Instantâneo de Vagas {avisovagas.IdProcessamentoJornalVagas} está sendo processado!");

                            avisovagas.Processar();
                            
                            _logger.Debug($"Aviso Instantâneo de Vagas {avisovagas.IdProcessamentoJornalVagas} processado!");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Aviso Instantâneo de Vagas {avisovagas.IdProcessamentoJornalVagas} com erro!", ex);

                            GravaLogText($"Aviso Instantâneo de Vagas {avisovagas.IdProcessamentoJornalVagas} com erro: {ex.Message}");

                            var guid = GerenciadorException.GravarExcecao(ex);
                            LogErroJornal.Logar($"Aviso Instantâneo de Vagas {ex.Message} Guid: {guid}", avisovagas.CodigoCurriculos, avisovagas.CodigoVagas);
                        }
                    });

                    GravaLogText($"Fim Processamento às -> { DateTime.Now.ToString() }");
                    _logger.Debug("Aviso Instantâneo de Vagas Terminou...");
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex, "Erro Execute()");
                    _logger.Debug("Aviso Instantâneo de Vagas de Erro... Execute()");
                }
            }

            #region MaxDegreeOfParallelism
            private int MaxDegreeOfParallelism()
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"].ToString());
            }
            #endregion

            #region GravaLogText
            private static void GravaLogText(string mensagem)
            {
                using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
                {
                    sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
                }
            }
            #endregion

        }
        #endregion


    }
}
