using System;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class Subscriber : EventingBasicConsumer, ISubscriber
    {
        private readonly ILog _logger;
        private readonly Action<BasicDeliverEventArgs> _callback;
        private readonly IModel _model;

        public Subscriber(IModel model, ILog logger, Action<BasicDeliverEventArgs> callback, string queuename) : base(model)
        {
            _callback = callback;
            _logger = logger;
            _model = model;

            ConsumerTag = BasicConsume(queuename, false);

            Received += Worker_Received;
        }

        private void Worker_Received(object sender, BasicDeliverEventArgs e)
        {
            _logger.Debug($"'{ConsumerTag} received a message at {DateTime.Now}");

            BasicAck(e.DeliveryTag, false);

            try
            {
                _callback(e);
            }
            catch (Exception exception)
            {
                BasicNack(e.DeliveryTag, false, true);
                _logger.Error(exception);
            }
        }

        public string BasicConsume(string queueName, bool noAck)
        {
            _model.BasicQos(0, 1, false);

            return _model.BasicConsume(queueName, noAck, this);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _model.BasicAck(deliveryTag, multiple);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _model.BasicNack(deliveryTag, multiple, requeue);
        }
    }
}