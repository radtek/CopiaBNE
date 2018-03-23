using BNE.Services.JornalVagas.MessageBroker.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class Publisher : IPublisher
    {
        private readonly IModel _model;

        public Publisher(IModel model)
        {
            _model = model;
        }

        public void Send(string exchange, string routingkey, byte[] body, bool persistent = false)
        {
            _model.BasicPublish(exchange, routingkey, false, new BasicProperties {Persistent = persistent}, body);
        }
    }
}