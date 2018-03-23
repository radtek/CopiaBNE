using System;
using System.Reactive;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.EventModel;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core.Interface
{
    public interface IChatListener : IDisposable
    {
        IObservable<EventPattern<ChatDefaultEventArgs>> OnClientTyping(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatEmptyEventArgs>> OnKeepAlive(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatResultEventArgs>> OnSendMessageTo(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatResultEventArgs>> OnRequestHistory(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatResultEventArgs>> OnRequestNewChatWith(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatDefaultEventArgs>> OnCloseChat(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatDefaultEventArgs>> OnDeleteChat(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatEmptyEventArgs>> OnNewConnection(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatEmptyEventArgs>> OnCloseConnection(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatEmptyEventArgs>> OnReconnectConnection(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatResultEventArgs>> OnGetMoreInfo(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatResultEventArgs>> OnRequestOnlineContacts(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<EventPattern<ChatDefaultEventArgs>> OnSendReadNotification(ObservablePriority eventPosition = ObservablePriority.Last);

        IObservable<EventPattern<EventArgs>> OnEndOfSession(ObservablePriority eventPosition = ObservablePriority.Last);
        IObservable<IChatListener> OnDisposed();

        IHubContext GetHubContext();

    }
}