using BNE.Services.JornalVagas.MessageBroker.Contracts;
using RabbitMQ.Client;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class BrokerConnection : IBrokerConnection
    {
        public BrokerConnection() : this(RabbitMQConfiguration.Default)
        {
        }

        public BrokerConnection(RabbitMQConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.Host,
                Port = configuration.Port,
                UserName = configuration.Username,
                Password = configuration.Password,
                VirtualHost = configuration.VirtualHost,
                AutomaticRecoveryEnabled = configuration.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = configuration.TopologyRecoveryEnabled,
                RequestedHeartbeat = configuration.RequestedHeartbeat,
                NetworkRecoveryInterval = configuration.NetworkRecoveryInterval,
                ContinuationTimeout = configuration.ContinuationTimeout
            };

            Connection = factory.CreateConnection();
        }

        public IConnection Connection { get; private set; }
    }
}