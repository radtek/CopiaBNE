using System;
using RabbitMQ.Client.Events;

namespace BNE.Services.JornalVagas.MessageBroker.Contracts
{
    public interface IListener
    {
        IPublisher Publisher { get; }
        void Subscribe(Action<BasicDeliverEventArgs> callback);
    }
}