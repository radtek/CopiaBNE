using System;
using System.ServiceProcess;
using BNE.BLL;
using BNE.EL;
using Quartz;
using Quartz.Impl;

namespace BNE.Services
{
    partial class QuantidadeInsuficienteRetornoPesquisaCurriculo : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public QuantidadeInsuficienteRetornoPesquisaCurriculo()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                PesquisaCurriculo.NotificarQuantidadeInsuficienteDeCurriculo();
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

            var job = JobBuilder.Create<QuantidadeInsuficienteRetornoPesquisaCurriculo>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 6, 0))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
    }
}