using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BNE.BLL.Notificacao;
using log4net;
using Quartz;

namespace BNE.Services.JornalVagas
{
    [DisallowConcurrentExecution]
    public class JornalVagasJob : IJob
    {
        private readonly JornalVagasService _jornalService = new JornalVagasService();
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly int MaxTask = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTask"]);

        public JornalVagasJob()
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("Job started...");
            _logger.Debug("Job started now " + DateTime.Now);

            try
            {
                var tasks = new List<Task>();
                var queue = new BlockingCollection<ProcessamentoJornalVagas>();

                Task producer = Task.Factory.StartNew(() =>
                {
                    foreach (var item in _jornalService.GetAllToProcess())
                    {
                        queue.Add(item);
                    }

                    queue.CompleteAdding();
                }, TaskCreationOptions.LongRunning);
                tasks.Add(producer);

                for (int x = 0; x < MaxTask; x++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        while (!queue.IsCompleted)
                        {
                            _logger.Info($"{queue.Count} itens na fila...");
                            ProcessamentoJornalVagas item;
                            if (queue.TryTake(out item))
                            {
                                Processar(item);
                            }
                        }
                    }, TaskCreationOptions.LongRunning));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao recuperar jornais para processar", ex);
                LogErroJornal.Logar(ex.Message, string.Empty, string.Empty);
            }

            _logger.Debug("Job ended...");
        }

        private void Processar(ProcessamentoJornalVagas jornal)
        {
            try
            {
                _logger.Debug($"Jornal {jornal.IdProcessamentoJornalVagas} está sendo processado!");

                jornal.Processar();

                _logger.Debug($"Jornal {jornal.IdProcessamentoJornalVagas} processado!");
            }
            catch (Exception ex)
            {
                jornal.FinalizarProcessamento();

                _logger.Error($"Jornal {jornal.IdProcessamentoJornalVagas} com erro!", ex);
                LogErroJornal.Logar(ex.Message, jornal.CodigoCurriculos, jornal.CodigoVagas);
            }
        }
    }
}