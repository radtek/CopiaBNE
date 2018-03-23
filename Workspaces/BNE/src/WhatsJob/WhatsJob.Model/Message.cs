using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsJob.Model
{
    public class Message
    {
        public int Id { get; set; }

        public string WhatsId { get; set; }

        public Channel WhatsChannel { get; set; }

        public Contact Contact { get; set; }

        public string TextMessage { get; set; }

        public bool Received { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ReceivedByServer { get; set; }

        public DateTime? ReceivedByClient { get; set; }

        public DateTime? ReadByClient { get; set; }

        public bool Replied { get; set; }

        public bool SentToServer { get; set; }

        public Message() { }

        public Message(string whatsId, Channel whatsChannel, Contact contact, string textMessage, bool received)
        {
            this.WhatsId = whatsId;
            this.Date = DateTime.Now;
            this.Contact = contact;
            this.WhatsChannel = whatsChannel;
            this.TextMessage = textMessage;
            this.Received = received;
            this.Replied = this.SentToServer = false;
            this.ReadByClient = this.ReceivedByClient = this.ReceivedByServer = null;
        }
    }
}
