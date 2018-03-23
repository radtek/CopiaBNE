using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using Quartz;
using Quartz.Impl;
using System.Threading;

namespace BNE.Services
{
    partial class EnviarNotaAntecipada : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public EnviarNotaAntecipada()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou o EnviarNotaAntecipada em " + DateTime.Now);
            #region [Envio das notas Antecipadas]

            Pagamento.EnvioNotaAntecipadaSemPagamento();
            Thread.Sleep(new TimeSpan(0, 10, 0));//enviar as notas que ainda não tinham sido geradas.
            Pagamento.EnvioNotaAntecipadaSemPagamento();

            #endregion
            GravaLogText("Terminou o EnviarNotaAntecipada em " + DateTime.Now);
        }
       
        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo EnviarNotaAntecipada.");

            _scheduler.Start();

            var job = JobBuilder.Create<EnviarNotaAntecipada>()
                .Build();
            DayOfWeek[] dias = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(7, 0, dias))
                .Build();

            _scheduler.ScheduleJob(job, trigger);

        }

        protected override void OnStop()
        {
            GravaLogText("EnviarNotaAntecipada desativado.");
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
