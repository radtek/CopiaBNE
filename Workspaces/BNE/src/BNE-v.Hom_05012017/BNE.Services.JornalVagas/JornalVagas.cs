using System.ServiceProcess;
using Quartz;
using Quartz.Impl;

namespace BNE.Services.JornalVagas
{
    [DisallowConcurrentExecution]
    partial class JornalVagas : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public JornalVagas()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            BLL.Notificacao.JornalVagas.ProcessarJornal();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();

            var job = JobBuilder.Create<JornalVagas>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("JornalVagas")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.CronSchedule("0 0-59/1 * * * ?"))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
    }
}