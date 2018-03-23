using System;
using System.ServiceProcess;
using BNE.BLL;
using BNE.EL;
using Quartz;
using Quartz.Impl;
using System.IO;

namespace BNE.Services
{
    partial class NotificacaoPesquisaCurriculoAtendimento : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public NotificacaoPesquisaCurriculoAtendimento()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GravaLogText("Iniciou o NotificacaoPesquisaCurriculoAtendimento em " + DateTime.Now);
                PesquisaCurriculo.NotificacaoPesquisaCurriculoAtendimento(DateTime.Now.Date.AddDays(-1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day, 23, 59, 59));
                GravaLogText("Finalizou o NotificacaoPesquisaCurriculoAtendimento em " + DateTime.Now);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();

            DayOfWeek[] dias = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Friday,DayOfWeek.Saturday,DayOfWeek.Sunday };
            var job = JobBuilder.Create<NotificacaoPesquisaCurriculoAtendimento>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(7, 0, dias))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
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