using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using Quartz;
using Quartz.Impl;


namespace BNE.Services
{
    partial class InscritosSTC : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public InscritosSTC()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou o InscritosSTC em " + DateTime.Now);
            Filial.CartaInscritos();
            GravaLogText("Terminou o InscritosSTC em " + DateTime.Now);
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo InscritosSTC.");
            _scheduler.Start();
            var job = JobBuilder.Create<InscritosSTC>().Build();

            var trigger =
                TriggerBuilder.Create ()
                    .WithIdentity("InscritosSTC")
                    .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 6 L * ?")).Build();

            _scheduler.ScheduleJob(job, trigger);
          
        }

        protected override void OnStop()
        {
            GravaLogText("ServicoInscritosSTC desativado.");
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
