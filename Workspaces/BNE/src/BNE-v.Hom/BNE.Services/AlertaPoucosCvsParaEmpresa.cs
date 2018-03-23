using System;
using System.IO;
using System.ServiceProcess;
using BNE.BLL;
using Quartz;
using Quartz.Impl;


namespace BNE.Services
{
    partial class AlertaPoucosCvsParaEmpresa : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public AlertaPoucosCvsParaEmpresa()
        {
            InitializeComponent();
        }
               
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                GravaLogText("Iniciou o AlertaPoucosCvsParaEmpresa em " + DateTime.Now);
                VagaCandidato.EmailEmpresaVagaPoucosCVs();
                GravaLogText("Terminou o AlertaPoucosCvsParaEmpresa em " + DateTime.Now);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "erro ao executar robo de AlertaPoucosCvsParaEmpresa");
            }
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo AlertaPoucosCvsParaEmpresa.");
            _scheduler.Start();

            var job = JobBuilder.Create<AlertaPoucosCvsParaEmpresa>()
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
            GravaLogText("AlertaPoucosCvsParaEmpresa desativado.");
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


