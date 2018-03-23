using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BNE.Chat.Core.Interface;
using BNE.Chat.Helper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core.Hubs
{
    [HubName("celChatServer")]
    public class ChatHub : Hub
    {
        #region [ Empty Chat Publisher ]
        private class EmptyChatPublisher : IChatPublisher
        {
            public static readonly IChatPublisher Default = new EmptyChatPublisher();
            private EmptyChatPublisher()
            {

            }
            public void RaiseSessionEnd(object realSender, EventArgs realArgs)
            {

            }
            public Task RaiseClientTyping(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyTask;
            }
            public Task RaiseKeepAlive(Hub chatHub)
            {
                return TaskAsyncHelper.EmptyTask;
            }
            public Task<ISignalRGenericResult> RaiseSendMessageTo(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyGenericResult;
            }
            public Task<ISignalRGenericResult> RaiseRequestHistory(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyGenericResult;
            }
            public Task<ISignalRGenericResult> RaiseRequestNewChatWith(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyGenericResult;
            }

            public Task<ISignalRGenericResult> RaiseGetMoreInfo(Hub chatHub, object receiveArgs)
            {
                return TaskAsyncHelper.EmptyGenericResult;
            }

            public Task<ISignalRGenericResult> RaiseRequestOnlineContacts(Hub chatHub)
            {
                return TaskAsyncHelper.EmptyGenericResult;
            }

            public Task RaiseCloseChat(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyTask;
            }
            public Task RaiseNewConnection(Hub currentHub)
            {
                return TaskAsyncHelper.EmptyTask;
            }
            public Task RaiseReconnectConnection(Hub currentHub)
            {
                return TaskAsyncHelper.EmptyTask;
            }
            public Task RaiseCloseConnection(Hub currentHub)
            {
                return TaskAsyncHelper.EmptyTask;
            }

            public Task RaiseSendReadNotification(Hub chatHub, object receiveArgs)
            {
                return TaskAsyncHelper.EmptyTask;
            }

            public Task RaiseDeleteChat(Hub chatHub, object chatValueArgs)
            {
                return TaskAsyncHelper.EmptyTask;
            }
        }
        #endregion

        private readonly IChatPublisher _manager;

        public ChatHub(IChatPublisher manager)
        {
            if (manager != null)
            {
                this._manager = manager;
                return;
            }

            Trace.WriteLine("Warning! 'ChatHub' will not work correctly, there is a invalid 'IChatPublisher'.");
            this._manager = EmptyChatPublisher.Default;
        }

        public override Task OnConnected()
        {
            var task = this._manager.RaiseNewConnection(this);
            if (task == null)
                return base.OnConnected();

            return task;
        }

        public override Task OnDisconnected()
        {
            var task = this._manager.RaiseCloseConnection(this);
            if (task == null)
                return base.OnDisconnected();

            return task;
        }

        public override Task OnReconnected()
        {
            var task = this._manager.RaiseReconnectConnection(this);
            if (task == null)
                return base.OnDisconnected();

            return task;
        }

        public Task KeepAlive()
        {
            var res = _manager.RaiseKeepAlive(this);
            if (res == null)
                return TaskAsyncHelper.EmptyTask;

            return res;
        }

        public Task ClientTyping(object receiveArgs)
        {
            var res = _manager.RaiseClientTyping(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyTask;

            return TaskAsyncHelper.EmptyTask;
        }

        public Task CloseChat(object receiveArgs)
        {
            var res = _manager.RaiseCloseChat(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyTask;

            return res;
        }

        public Task DeleteContact(object receiveArgs)
        {
            var res = _manager.RaiseDeleteChat(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyTask;

            return res;
        }

        public Task<ISignalRGenericResult> RequestOnlineContacts()
        {
            var res = _manager.RaiseRequestOnlineContacts(this);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }

        public Task<ISignalRGenericResult> SendMessage(object receiveArgs)
        {
            var res = _manager.RaiseSendMessageTo(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }

        public Task<ISignalRGenericResult> RequestChatWith(object receiveArgs)
        {
            var res = _manager.RaiseRequestNewChatWith(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }

        public Task<ISignalRGenericResult> RequestHistory(object receiveArgs)
        {
            var res = _manager.RaiseRequestHistory(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }

        public Task<ISignalRGenericResult> GetMoreInfo(object receiveArgs)
        {
            var res = _manager.RaiseGetMoreInfo(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }

        public Task SendReadNotification(object receiveArgs)
        {
            var res = _manager.RaiseSendReadNotification(this, receiveArgs);
            if (res == null)
                return TaskAsyncHelper.EmptyGenericResult;

            return res;
        }


    }
}
