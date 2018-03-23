using BNE.Chat.Core;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;

namespace BNE.Test.App.Chat
{
    public class EmptyChatProducer : BNEChatProducerBase
    {
        public EmptyChatProducer(IChatListener manager, ChatStore chatStore, NotificationHandler handler)
            : base(manager, chatStore, handler)
        {
        }
    }
}