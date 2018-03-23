using System;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using BNE.Services.Properties;
using Quartz;
using Quartz.Impl;


namespace BNE.Services
{
    partial class BoletosRecorrentesAVencer : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private readonly int _horaExecucao = Settings.Default.EnvioBoletoRecorrentes;
        private readonly int _minutoExecucao = Settings.Default.EnvioBoletoRecorrentesDalay;

        public BoletosRecorrentesAVencer()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou o Serviço de Boletos Recorrentes em " + DateTime.Now);
            IniciarEnvio();
            GravaLogText("Iniciou o Serviço de Boletos Recorrentes em " + DateTime.Now);
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo.");
            _scheduler.Start();
            var job = JobBuilder.Create<BoletosRecorrentesAVencer>().Build();

            var trigger =
                TriggerBuilder.Create()
                    .WithIdentity("BoletosRecorrentesAVencer")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(_horaExecucao, _minutoExecucao)).Build();

            _scheduler.ScheduleJob(job, trigger);
            base.OnStart(args);
        }

        private static void GravaLogText(string mensagem)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + mensagem);
            }
        }

        public void IniciarEnvio()
        {   
            Pagamento.EnviarEmailComBoletosRecorrentes();
        }
    }
}
