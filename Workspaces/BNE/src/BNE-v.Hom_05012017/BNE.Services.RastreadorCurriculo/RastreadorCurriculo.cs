using System.ServiceProcess;
using Quartz;
using Quartz.Impl;

namespace BNE.Services.RastreadorCurriculo
{
    [DisallowConcurrentExecution]
    internal partial class RastreadorCurriculo : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public RastreadorCurriculo()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            BLL.RastreadorCurriculo.Processar();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();

            var job = JobBuilder.Create<RastreadorCurriculo>()
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("RastreadorCurriculo")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 0/1 1/1 * ? *"))
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
    }
}