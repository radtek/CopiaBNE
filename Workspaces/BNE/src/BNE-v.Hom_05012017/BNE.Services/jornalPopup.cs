using BNE.EL;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Services
{

    partial class jornalPopup : ServiceBase, IJob
    {

        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        public jornalPopup()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();

            var job = JobBuilder.Create<jornalPopup>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(6, 0, DayOfWeek.Tuesday, DayOfWeek.Thursday))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var teste = new BLL.Notificacao.ProcessamentoJornalPopup();
                teste.IniciarProcessoPopup();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
     
    }
}
