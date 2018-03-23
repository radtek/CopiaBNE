using System;
using System.ServiceProcess;
using BNE.BLL;
using BNE.EL;
using Quartz;
using Quartz.Impl;

namespace BNE.Services
{
    partial class AlertaTentativaCompra : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public AlertaTentativaCompra()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                TransacaoResposta.AlertaTentativaCompra();
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

            var job = JobBuilder.Create<AlertaTentativaCompra>()
                .Build();
            DayOfWeek[] dias = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(5, 0, dias))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
    }
}