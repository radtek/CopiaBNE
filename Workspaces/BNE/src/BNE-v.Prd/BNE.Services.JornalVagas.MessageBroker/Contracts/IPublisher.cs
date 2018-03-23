namespace BNE.Services.JornalVagas.MessageBroker.Contracts
{
    public interface IPublisher
    {
        void Send(string exchange, string routingkey, byte[] body, bool persistent = false);
    }
}