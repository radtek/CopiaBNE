using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.Chat.Helper;

namespace BNE.Chat.Model
{
    public class PrivateHistoryChat
    {
        #region [ Static ]
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> HistoryLimitQuantity =
            new HardConfig<int>("chat_quantity_of_message_in_history", 100).Wrap(a => a.Value);

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> HistoryLimitTime =
            new HardConfig<int>("chat_minimum_time_of_history", 10).Wrap(a => a.Value);
        #endregion

        #region [ Atributos ]
        private ImmutableList<PrivateHistoryMessage> _messages;
        private readonly DateTime _creationDate = DateTime.Now;
        private DateTime _lastUse = DateTime.Now;
        private readonly object _syncRoot = new object();
        #endregion

        #region [ Properties ]
        public int PartyWith { get; set; }
        public int OwnerId { get; set; }

        public bool IsEmpty
        {
            get
            {
                return _messages.Count == 0;
            }
        }

        public bool MinimumLifetimeReached
        {
            get { return (DateTime.Now - _creationDate).TotalMinutes > HistoryLimitTime.Value; }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
        }

        public string GuidChat { get; set; }

        public DateTime LastUse
        {
            get { return _lastUse; }
        }
        #endregion

        #region [ Construtor ]
        public PrivateHistoryChat()
        {
            GuidChat = Guid.NewGuid().ToString();
            _messages = new ImmutableList<PrivateHistoryMessage>(Enumerable.Empty<PrivateHistoryMessage>());
        }

        public PrivateHistoryChat(IEnumerable<PrivateHistoryMessage> content)
        {
            GuidChat = Guid.NewGuid().ToString();
            _messages = new ImmutableList<PrivateHistoryMessage>(content);
        }
        #endregion

        #region [ Public ]
        public IEnumerable<PrivateHistoryMessage> GetMessageHistory()
        {
            foreach (var item in _messages)
            {
                _lastUse = DateTime.Now;
                yield return item;
            }
        }

        public bool AddMessage(PrivateHistoryMessage message)
        {
            _lastUse = DateTime.Now;

            if (message == null)
                throw new NullReferenceException("message");

            lock (_syncRoot)
            {
                if (_messages.Any(a => a.Guid == message.Guid))
                    return false;

                var lastMessage = _messages.LastOrDefault();

                if (lastMessage != null &&
                    lastMessage.CreatorType == PrivateHistoryMessage.MessageOwner.Other
                    && message.CreatorType == PrivateHistoryMessage.MessageOwner.Other)
                {
                    if (message.StatusDate > lastMessage.StatusDate)
                    {
                        if (message.StatusDate - lastMessage.StatusDate < TimeSpan.FromSeconds(2))
                        {
                            lastMessage.MessageContent = lastMessage.MessageContent + message.MessageContent;
                            return false;
                        }
                    }
                    else
                    {
                        if (lastMessage.StatusDate - message.StatusDate < TimeSpan.FromSeconds(2))
                        {
                            lastMessage.MessageContent = message.MessageContent + lastMessage.MessageContent;
                            return false;
                        }
                    }
                }
                else
                {
                    var firstMessage = _messages.FirstOrDefault();

                    if (firstMessage != null
                        && firstMessage.CreatorType == PrivateHistoryMessage.MessageOwner.Other
                        && message.CreatorType == PrivateHistoryMessage.MessageOwner.Other
                        && firstMessage.StatusDate >= message.StatusDate
                        && firstMessage.StatusDate - message.StatusDate < TimeSpan.FromSeconds(2))
                    {
                        firstMessage.MessageContent = message.MessageContent + firstMessage.MessageContent;
                        return false;
                    }
                }

                if (_messages.Count > HistoryLimitQuantity.Value)
                {
                    _messages = _messages.Remove(_messages.First());
                }

                _messages = _messages.Add(message);

                if (lastMessage != null
                    && message.CreationDate < lastMessage.CreationDate)
                {
                    _messages = _messages.OrderBy(a => a.CreationDate).AsImmutable();
                }
            }

            return true;
        }
        #endregion

        public PrivateHistoryMessage AddMessage(string guid, bool writerIsTheOwner, string content, DateTime creationDate, DateTime statusDate, int statusValue)
        {
            var privateHistory = new PrivateHistoryMessage
            {
                CreatorType = writerIsTheOwner ? PrivateHistoryMessage.MessageOwner.Self : PrivateHistoryMessage.MessageOwner.Other,
                Guid = guid,
                CreationDate = creationDate,
                StatusTypeValue = statusValue,
                StatusDate = statusDate,
                MessageContent = content
            };

            if (AddMessage(privateHistory))
                return privateHistory;

            return null;
        }
    }
}
