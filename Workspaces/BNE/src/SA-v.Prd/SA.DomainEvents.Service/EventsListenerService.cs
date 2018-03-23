using System.Reflection;
using log4net;
using SA.DomainEvents.Service.Handlers.BNE;
using SA.WebPush.Interfaces;
using SharedKernel.DomainEvents.CrossDomainEvents;

namespace SA.DomainEvents.Service
{
    public class EventsListenerService
    {
        private readonly ILog _logger;
        private readonly IWebPushService _webPushService;

        public EventsListenerService(IWebPushService webPushService, ILog logger)
        {
            _webPushService = webPushService;
            _logger = logger;
        }

        public void Start()
        {
            _logger.Info($"Starting {GetType().FullName} Listener Service");

            Listener.AddHandler(new OnVisualizacaoCurriculoSemSaldoHandler(_logger, _webPushService));

            _logger.Info($"{GetType().FullName} Listener Service Started");
        }

        public void Stop()
        {
            _logger.Info($"Stopping {GetType().FullName} Listener");

            Listener.CloseConnections();

            _logger.Info($"{GetType().FullName} Listener Stopped");
        }
    }
}
