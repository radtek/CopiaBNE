using Quartz;
using Quartz.Impl;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace BNE.Services.NotificacoesVip
{
    partial class NotificacoesVip : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private readonly int _horaExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["horaExecucao"]);
        private readonly int _minutoExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["minutoExecucao"]);

        public NotificacoesVip()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou agora o envio das Notificações VIP");

            BNE.BLL.NotificacoesVip.NotificacoesVip.IniciarNotificacoesVip();

            GravaLogText("Terminou agora o envio das Notificações VIP");
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Servico ativado.");
            _scheduler.Start();
            var job = JobBuilder.Create<NotificacoesVip>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("NotificacoesVip")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(_horaExecucao, _minutoExecucao))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            GravaLogText("Servico desativado.");
            _scheduler.Shutdown();
        }

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
}
