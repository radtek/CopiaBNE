using RabbitMQ.Client;

namespace BNE.Services.JornalVagas.MessageBroker.Contracts
{
    public interface IBrokerConnection
    {
        IConnection Connection { get; }
    }
}