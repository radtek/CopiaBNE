using System;

namespace BNE.Chat.Core.Interface
{
    public interface INotifyAppBeginRequest
    {
        event EventHandler ReceiveAppBeginRequest;
    }
}