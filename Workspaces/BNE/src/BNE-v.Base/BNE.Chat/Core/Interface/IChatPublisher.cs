using System;
using System.Threading.Tasks;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using Microsoft.AspNet.SignalR;

namespace BNE.Chat.Core.Interface
{
    public interface IChatPublisher
    {
        void RaiseSessionEnd(object realSender, EventArgs realArgs);
        Task RaiseClientTyping(Hub chatHub, object chatValueArgs);
        Task RaiseKeepAlive(Hub chatHub);
        Task<ISignalRGenericResult> RaiseSendMessageTo(Hub chatHub, object chatValueArgs);
        Task<ISignalRGenericResult> RaiseRequestHistory(Hub chatHub, object chatValueArgs);
        Task<ISignalRGenericResult> RaiseRequestNewChatWith(Hub chatHub, object chatValueArgs);
        Task<ISignalRGenericResult> RaiseGetMoreInfo(Hub chatHub, object receiveArgs);
        Task<ISignalRGenericResult> RaiseRequestOnlineContacts(Hub chatHub);

        Task RaiseSendReadNotification(Hub chatHub, object receiveArgs);
        Task RaiseCloseChat(Hub chatHub, object chatValueArgs);

        Task RaiseDeleteChat(Hub chatHub, object chatValueArgs);

        Task RaiseNewConnection(Hub currentHub);
        Task RaiseReconnectConnection(Hub currentHub);
        Task RaiseCloseConnection(Hub currentHub);



    }
}