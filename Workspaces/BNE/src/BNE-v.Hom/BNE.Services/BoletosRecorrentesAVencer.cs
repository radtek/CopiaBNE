using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using Quartz;
using Quartz.Impl;


namespace BNE.Services
{
    partial class BoletosRecorrentesAVencer : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private readonly int _horaExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["horaExecucaoBoleto"]);
        private readonly int _minutoExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["minutoExecucaoBoleto"]);

        public BoletosRecorrentesAVencer()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou o Serviço de Boletos Recorrentes em " + DateTime.Now);
            BLL.Pagamento.EnviarEmailComBoletosRecorrentes();
            GravaLogText("Terminou o Serviço de Boletos Recorrentes em " + DateTime.Now);
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo BoletoRecorrente.");
            _scheduler.Start();
            var job = JobBuilder.Create<BoletosRecorrentesAVencer>().Build();

            var trigger =
                TriggerBuilder.Create()
                    .WithIdentity("BoletosRecorrentesAVencer")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(_horaExecucao, _minutoExecucao)).Build();

            _scheduler.ScheduleJob(job, trigger);
          
        }

        protected override void OnStop()
        {
            GravaLogText("Servico Boleto Recorrente desativado.");
            _scheduler.Shutdown();
        }

        private static void GravaLogText(string mensagem)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
            }
        }

       
    }
}
