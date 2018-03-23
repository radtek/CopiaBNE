using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using BNE.BaseService;
using BNE.BLL.Notificacao;
using BNE.JornalVagas;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using Quartz;

namespace BNE.Services.JornalVagas.Enfileirar
{
    public class JornalVagasJob : AbstractJob
    {
        private readonly JornalVagaService _jornalService;
        private readonly IListener _listener;
        private readonly ISerializer _serializer;

        public JornalVagasJob(JornalVagaService jornalService, ILog logger, ISerializer serializer, IBrokerConnection brokerConnection) : base(logger)
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;

            _jornalService = jornalService;
            _serializer = serializer;

            _listener = new Listener(Constants.ParaProcessarQueuename, brokerConnection, logger);
        }

        public override void Execute()
        {
            try
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

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

                for (int x = 0; x < MaxWorkers(); x++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        while (!queue.IsCompleted)
                        {
                            ProcessamentoJornalVagas item;
                            if (queue.TryTake(out item))
                            {
                                Enfileirar(item);
                            }
                        }
                    }, TaskCreationOptions.LongRunning));
                }

                Task.WaitAll(tasks.ToArray());

                stopwatch.Stop();

                _logger.Info($"Time to queue {stopwatch.Elapsed}");
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao recuperar jornais para processar", ex);
            }
        }

        public void Enfileirar(ProcessamentoJornalVagas jornal)
        {
            try
            {
                _logger.Debug($"Jornal {jornal} está sendo enfileirado!");

                _jornalService.IniciarProcessamento(jornal);

                try
                {
                    _listener.Publisher.Send(string.Empty, Constants.ParaProcessarQueuename, _serializer.Serialize(jornal), true);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Jornal {jornal} com erro!", ex);
                    _jornalService.Reprocessar(jornal);
                }

                _logger.Debug($"Jornal {jornal} enfileirado!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Jornal {jornal} com erro!", ex);
            }
        }

        private static int MaxWorkers()
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["MaxWorkers"]);
            }
            catch (Exception)
            {
                return 1;
            }
        }
       
    }
}