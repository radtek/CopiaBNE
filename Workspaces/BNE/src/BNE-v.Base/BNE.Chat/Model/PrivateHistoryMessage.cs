using System;

namespace BNE.Chat.Model
{
    public class PrivateHistoryMessage
    {
        public enum MessageOwner
        {
            Self,
            Other
        }

        private DateTime _statusDate;

        public PrivateHistoryMessage()
        {
            CreationDate = DateTime.Now;
        }

        public MessageOwner CreatorType { get; set; }

        public string MessageContent { get; set; }

        public DateTime CreationDate { get; set; }

        public string Guid { get; set; }

        public int StatusTypeValue { get; set; }

        public DateTime StatusDate
        {
            get { return _statusDate; }
            set
            {
                _statusDate = value;
            }
        }

    }
}