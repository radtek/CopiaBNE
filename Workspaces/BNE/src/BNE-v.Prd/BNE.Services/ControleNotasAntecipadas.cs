using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using Quartz;
using Quartz.Impl;

namespace BNE.Services
{
    partial class ControleNotasAntecipadas : ServiceBase, IJob
    {
        private readonly IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();

        private readonly int _horaExecucao =
            Convert.ToInt32(ConfigurationManager.AppSettings["horaExecucaoNotaAntecipada"]);

        private readonly int _minutoExecucao =
            Convert.ToInt32(ConfigurationManager.AppSettings["minutoExecucaoNotaAntecipada"]);

        private readonly int _diaExecucao =
            Convert.ToInt32(ConfigurationManager.AppSettings["diaExecucaoNotaAntecipada"]);

        public ControleNotasAntecipadas()
        {
            InitializeComponent();
        }

        public void Execute(IJobExecutionContext context)
        {
            GravaLogText("Iniciou o Serviço de Notas Fiscais Antecipadas em " + DateTime.Now);
            
            var notasAntecipadasSemPagamento = BLL.Pagamento.NotasAntecipadasSemPagamento();

            BLL.Pagamento.EnviarEmailNotasAntecipadasSemPagamento(notasAntecipadasSemPagamento);

            GravaLogText("Terminou o Serviço de Notas Fiscais Antecipadas em " + DateTime.Now);
        }

        protected override void OnStart(string[] args)
        {
            GravaLogText("Serviço ativo ControleNotasAntecipadas.");
            _scheduler.Start();
            var job = JobBuilder.Create<ControleNotasAntecipadas>().Build();

            var trigger =
                TriggerBuilder.Create()
                    .WithIdentity("ControleNotasAntecipadas")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(_diaExecucao, _horaExecucao, _minutoExecucao)).Build();

            _scheduler.ScheduleJob(job, trigger);

        }

        protected override void OnStop()
        {
            GravaLogText("Servico ControleNotasAntecipadas desativado.");
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
