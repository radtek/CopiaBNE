using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BNE.Chat.Core.Interface;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core
{
    public class BNEChatClientProxy : IChatClientProxy
    {
        private readonly Lazy<dynamic> _lazyAccesor;
        private readonly dynamic _hubItem;

        protected BNEChatClientProxy(dynamic hubItem)
        {
            if (hubItem == null)
                throw new NullReferenceException("hubItem");

            this._hubItem = hubItem;
        }

        public BNEChatClientProxy(Func<dynamic> hubAccessor)
        {
            if (hubAccessor == null)
                throw new NullReferenceException("hubAccessor");

            _lazyAccesor = new Lazy<dynamic>(hubAccessor, LazyThreadSafetyMode.PublicationOnly);
        }

        protected dynamic Accessor
        {
            get
            {
                if (_lazyAccesor != null)
                {
                    return _lazyAccesor.Value;
                }

                return _hubItem;
            }
        }

        public virtual void SendMessage(object mensagemArgs)
        {
            Accessor.receiveChatMessage(mensagemArgs);
        }

        public virtual void SendCloseChat(object fimDoChatArgs)
        {
            Accessor.receiveEndChat(fimDoChatArgs);
        }

        public virtual void SendOnlineContacts(object contatosArgs)
        {
            Accessor.receiveOnlineContacts(contatosArgs);
        }

        public virtual void SendStatusOfMessage(object mensagemArgs)
        {
            Accessor.receiveMessageStatus(mensagemArgs);
        }

        public virtual void SendCloseConnection()
        {
            Accessor.forceStopConnection();
        }

        public virtual void SendOpossiteTypingSignal()
        {
            Accessor.receiveOppositeTypingSignal();
        }

        public void SendDeleteContact(int targetId)
        {
            Accessor.receiveDeleteContact(targetId);
        }
    }


    public static class BNEChatClientProxyHelper
    {
        public static IChatClientProxy CreateBroadcastProxy(this IHubConnectionContext hub)
        {
            return new BNEChatClientProxy(() => hub.All);
        }

        public static IChatClientProxy CreateBroadcastExceptProxy(this IHubConnectionContext hub, IEnumerable<string> connectionExceptions)
        {
            return new BNEChatClientProxy(() => hub.AllExcept(connectionExceptions.ToArray()));
        }

        public static IChatClientProxy CreatePrivateProxy(this IHubConnectionContext hub, string targetConnectionId)
        {
            return new BNEChatClientProxy(() => hub.Client(targetConnectionId));
        }

        public static IEnumerable<IChatClientProxy> CreatePrivateSelectionProxy(this IHubConnectionContext hub, IEnumerable<string> targets)
        {
            if (targets == null)
                throw new NullReferenceException("connection");

            foreach (var item in targets)
            {
                string closure = item;
                yield return new BNEChatClientProxy(() => hub.Client(closure));
            }
        }
    }
}