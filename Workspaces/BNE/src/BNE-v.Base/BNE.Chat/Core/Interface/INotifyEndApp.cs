using System;

namespace BNE.Chat.Core.Interface
{
    public interface INotifyEndApp
    {
        event EventHandler ReceiveEndOfApp;
    }
}