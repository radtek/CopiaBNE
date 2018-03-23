using System;

namespace BNE.Services.JornalVagas.MessageBroker.Contracts
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T data);
        object Deserialize(Type dataType, byte[] body);
    }
}
