using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BNE.BLL.Notificacao;
using BNE.JornalVagas;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;

namespace BNE.Services.JornalVagas.AtualizarAlerta
{
    public class ContainerService
    {
        private readonly ILog _logger;
        private readonly ISerializer _serializer;
        private readonly IBrokerConnection _brokerConnection;


        public ContainerService(ILog logger, ISerializer serializer, IBrokerConnection brokerConnection)
        {
            _logger = logger;
            _serializer = serializer;
            _brokerConnection = brokerConnection;
        }

        public void Start()
        {
            _logger.Debug($"{GetType().FullName}.Start()");

            var tasks = new List<Task>();
            for (var i = 0; i < MaxWorkers(); i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var argsEmpty = false;

                        using (var consumer = new Listener(Constants.ParaAtualizarAlertaQueuename, _brokerConnection, _logger))
                        {
                            var alertas = new ConcurrentBag<Alerta>();

                            while (!LimitsToSend(alertas) && !TimeToSend(alertas) && !argsEmpty)
                            {
                                try
                                {
                                    var getResult = consumer.Get();
                                    if (getResult?.Body != null)
                                    {
                                        var alerta = (BNE.JornalVagas.AtualizarAlerta)_serializer.Deserialize(typeof(BNE.JornalVagas.AtualizarAlerta), getResult.Body);
                                        alertas.Add(new Alerta(alerta.IdCurriculo));
                                    }
                                    else
                                    {
                                        argsEmpty = true;
                                    }
                                }
                                catch (Exception e)
                                {
                                    _logger.Error(e);
                                    throw;
                                }
                            }

                            if (alertas.Count > 0)
                            {
                                AlertaCurriculos.Atualizar(alertas.Select(p => p.IdCurriculo));
                            }

                            consumer.AckAll();
                        }

                        if (argsEmpty)
                        {
                            Thread.Sleep(5000);
                        }
                    }
                }, TaskCreationOptions.LongRunning));
            }

            Task.WhenAll(tasks);
        }

        private bool TimeToSend(ConcurrentBag<Alerta> alertas)
        {
            if (alertas.Count == 0)
                return false;

            return DateTime.Now.CompareTo(alertas.OrderBy(c => c.Created).First().Created.AddSeconds(30)) > 0;
        }

        private bool LimitsToSend(ConcurrentBag<Alerta> alertas)
        {
            return alertas.Count >= 100;
        }

        public void Stop()
        {
            _logger.Debug($"{GetType().FullName}.Stop()");
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

        public class Alerta
        {
            public int IdCurriculo { get; set; }
            public DateTime Created { get; private set; }

            public Alerta(int idCurriculo)
            {
                IdCurriculo = idCurriculo;
                Created = DateTime.Now;
            }
        }
    }

}