using RabbitMQ.Client;

namespace BNE.Services.JornalVagas.MessageBroker.Contracts
{
    public interface ISubscriber
    {
        string BasicConsume(string queueName, bool noAck);
        void BasicAck(ulong deliveryTag, bool multiple);
        void BasicNack(ulong deliveryTag, bool multiple, bool requeue);
    }
}
