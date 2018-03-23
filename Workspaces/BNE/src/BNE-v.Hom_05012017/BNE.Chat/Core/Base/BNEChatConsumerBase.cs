using System;
using System.Reactive;
using System.Reactive.Linq;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Interface;

namespace BNE.Chat.Core.Base
{
    public abstract class BNEChatConsumerBase : IDisposable
    {
        private readonly IChatListener _manager;
        private readonly IDisposable _sDisposable;

        protected virtual IChatListener Manager
        {
            get
            {
                return _manager;
            }
        }

        protected BNEChatConsumerBase(IChatListener manager)
        {
            if (manager == null)
                throw new NullReferenceException("manager");

            this._manager = manager;

            var compDisp = new System.Reactive.Disposables.CompositeDisposable();
            this._sDisposable = compDisp;

            var disp = this._manager.OnNewConnection().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(ConnectionOpened).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnCloseConnection().Do(ConnectionClosedUnsafe).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnSendMessageTo().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(NewMessage).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnSendReadNotification().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(NewReadNotification).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnRequestHistory().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(RequestHistory).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnRequestOnlineContacts().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(RequestOnlineContacts).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnGetMoreInfo().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(GetMoreInfo).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnDeleteChat().Where(a => IsValidToUse(a.Sender, a.EventArgs)).Do(DeleteChat).Subscribe();
            compDisp.Add(disp);

            disp = this._manager.OnEndOfSession().Do(EndOfSessionUnsafe).Subscribe();
            compDisp.Add(disp);
        }

        protected abstract void DeleteChat(EventPattern<ChatDefaultEventArgs> obj);

        protected abstract void NewReadNotification(EventPattern<ChatDefaultEventArgs> obj);

        protected abstract void GetMoreInfo(EventPattern<ChatResultEventArgs> obj);

        protected abstract void RequestOnlineContacts(EventPattern<ChatResultEventArgs> obj);

        protected abstract bool IsValidToUse(object sender, EventArgs eventArgs);

        protected abstract void EndOfSessionUnsafe(EventPattern<EventArgs> obj);

        protected abstract void RequestHistory(EventPattern<ChatResultEventArgs> requestParams);

        protected abstract void ConnectionOpened(EventPattern<ChatEmptyEventArgs> obj);

        protected abstract void ConnectionClosedUnsafe(EventPattern<ChatEmptyEventArgs> obj);

        protected abstract void NewMessage(EventPattern<ChatResultEventArgs> requestParams);

        public void Dispose()
        {
            Disposed(true);
        }

        protected virtual void Disposed(bool disposed)
        {
            var disp = _sDisposable;
            if (disp != null)
            {
                try
                {
                    disp.Dispose();
                }
                catch
                {
                }
            }

        }

        public virtual string GetDashboardLink()
        {
            return string.Empty;
        }
        public virtual int GetUsuarioFilialPerfil()
        {
            return 0;
        }
    }
}
