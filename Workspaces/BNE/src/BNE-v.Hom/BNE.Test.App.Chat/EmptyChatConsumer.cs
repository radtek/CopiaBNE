using System.Reactive;
using BNE.Chat.Core;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Test.App.Chat
{
    public class EmptyChatConsumer : BNEChatConsumerBase
    {
        public EmptyChatConsumer(IChatListener chatManager)
            : base(chatManager)
        {

        }
     

        protected override void ConnectionClosedUnsafe(System.Reactive.EventPattern<BNE.Chat.Core.EventModel.ChatEmptyEventArgs> obj)
        {
        }

        protected override void ConnectionOpened(System.Reactive.EventPattern<BNE.Chat.Core.EventModel.ChatEmptyEventArgs> obj)
        {

        }

        protected override void GetMoreInfo(EventPattern<ChatResultEventArgs> obj)
        {

        }

        protected override void RequestOnlineContacts(EventPattern<ChatResultEventArgs> obj)
        {

        }

        protected override bool IsValidToUse(object sender, EventArgs eventArgs)
        {
            return true;
        }

        protected override void EndOfSessionUnsafe(System.Reactive.EventPattern<EventArgs> obj)
        {

        }

        protected override void NewMessage(System.Reactive.EventPattern<BNE.Chat.Core.EventModel.ChatResultEventArgs> requestParams)
        {

        }

        protected override void RequestHistory(System.Reactive.EventPattern<BNE.Chat.Core.EventModel.ChatResultEventArgs> requestParams)
        {

        }


        protected override void NewReadNotification(EventPattern<ChatDefaultEventArgs> obj)
        {
        }

        protected override void DeleteChat(EventPattern<ChatDefaultEventArgs> obj)
        {
        }
    }
}
