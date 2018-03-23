using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class Listener : IListener, IDisposable
    {
        private readonly string _queuename;

        private readonly IModel _model;
        private readonly IPublisher _publisher;
        private readonly IBrokerConnection _brokerConnection;
        private readonly ILog _logger;
        private readonly List<ulong> _deliveryTags = new List<ulong>();
        private readonly List<ulong> _acks = new List<ulong>();

        private readonly ConcurrentBag<ISubscriber> _subscribers = new ConcurrentBag<ISubscriber>();

        public Listener(string queuename, IBrokerConnection brokerConnection, ILog logger)
        {
            _queuename = queuename;
            _logger = logger;
            _brokerConnection = brokerConnection;

            _model = brokerConnection.Connection.CreateModel();

            _publisher = Publisher();

            CreateQueue(_queuename);
        }

        public IPublisher Publisher()
        {
            _logger.Debug($"Creating new publisher to {_queuename}");

            var publisher = new Publisher(_model);

            return publisher;
        }

        IPublisher IListener.Publisher => _publisher;

        private void CreateQueue(string queueName)
        {
            _logger.Debug($"Creating new queue to {_queuename}");

            _model.QueueDeclare(queueName, true, false, false, null);

            _model.BasicQos(0, 1, false);
        }


        /// <summary>
        /// Used when a callback is defined
        /// </summary>
        /// <param name="callback"></param>
        public void Subscribe(Action<BasicDeliverEventArgs> callback)
        {
            var model = _brokerConnection.Connection.CreateModel();

            var worker = new Subscriber(model, _logger, callback, _queuename);

            _logger.Debug($"New worker {worker.ConsumerTag} added to {_queuename}");

            _subscribers.Add(worker);
        }

        /// <summary>
        /// Used when the client need to control the flow of acks
        /// </summary>
        /// <returns></returns>
        public BasicGetResult Get()
        {
            var basicGetResult = _model.BasicGet(_queuename, false);

            if (basicGetResult != null)
            {
                _deliveryTags.Add(basicGetResult.DeliveryTag);
            }
            return basicGetResult;
        }

        public void AckAll()
        {
            foreach (ulong ack in _deliveryTags)
            {
                _model.BasicAck(ack, false);
                _acks.Add(ack);
            }
        }

        public void Dispose()
        {
            foreach (ulong deliveryTag in _deliveryTags.Where(c => !_acks.Contains(c)))
            {
                _model.BasicNack(deliveryTag, false, true);
            }

            _model.Close(200, "Goodbye");
            _model?.Dispose();
        }
    }
}